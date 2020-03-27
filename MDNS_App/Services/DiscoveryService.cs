using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MDNS_App.Services
{
    public class DiscoveryService
    {
	    public event EventHandler<UdpReceiveResult> MessageReceived;

	    public void InitiateDnsMessageListener(UdpClient client)
	    {
		    Task.Run(async () =>
		    {
			    var result = await client.ReceiveAsync();
			    MessageReceived?.Invoke(this, result);
		    });
	    }
    }
}
