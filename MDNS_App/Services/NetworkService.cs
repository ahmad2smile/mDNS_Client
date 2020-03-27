using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MDNS_App.Services
{
    public class NetworkService
    {
        private const int MultiCastPort = 5353;
        private static readonly IPAddress MultiCastAddressIp4 = IPAddress.Parse("224.0.0.251");

        public UdpClient GetClient()
        {
            var client = new UdpClient(AddressFamily.InterNetwork); // TODO: Also run on IPv6

            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.Client.Bind(new IPEndPoint(IPAddress.Any, MultiCastPort));

            foreach (var localAddress in GetLocalAddresses())
            {
                client.Client.SetSocketOption(
                    SocketOptionLevel.IP,
                    SocketOptionName.AddMembership,
                    new MulticastOption(MultiCastAddressIp4, localAddress));
            }

            return client;
        }

        private static IEnumerable<IPAddress> GetLocalAddresses()
        {
            var activeNetworkInterface = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up);

            return activeNetworkInterface.SelectMany(n =>
                n.GetIPProperties().UnicastAddresses.Select(x => x.Address)
                        .Where(a =>
                            a.AddressFamily != AddressFamily.InterNetworkV6 &&
                            a.AddressFamily == AddressFamily.InterNetwork)
                );
        }
    }
}
