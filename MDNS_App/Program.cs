using System;
using System.Linq;
using Makaretu.Dns;
using MDNS_App.Services;

namespace MDNS_App
{
    class Program
    {
        static void Main(string[] args)
        {
            var networkService = new NetworkService();

            var client = networkService.GetClient();

            var discoveryService = new DiscoveryService();
            
            discoveryService.InitiateDnsMessageListener(client);

            Console.WriteLine("Initiated Discovery Service");

            discoveryService.MessageReceived += DnsMessageReceived;

            Console.ReadKey();
        }

        private static void DnsMessageReceived(object sender, System.Net.Sockets.UdpReceiveResult result)
        {
	        var message = new Message();
	        message.Read(result.Buffer, 0, result.Buffer.Length);
	        if (message.Questions == null || message.Questions.Count == 0)
	        {
		        return;
	        }

	        var question = message.Questions.First();

	        var domain = string.Join(".", question.Name.Labels.ToArray());

            Console.WriteLine($"Found Clients: {domain}");
        }
    }
}
