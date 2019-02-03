using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Models;
using Newtonsoft.Json;
using Process;
using Process.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusTopicPublisher
{
   
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static  NamespaceManager CreateNamespaceManager(string url,string issuerName, string issuerKey)
        {
            // Create the namespace manager which gives you access to
            // management operations
            var uri = ServiceBusEnvironment.CreateServiceUri(
                "sb", url, string.Empty);
            var tP = TokenProvider.CreateSharedAccessSignatureTokenProvider(
                issuerName, issuerKey);
            return new NamespaceManager(uri, tP);
        }



        public static async Task MainAsync()
        {
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo('A',ConsoleKey.A,false,false,false);
            string url = "testservicebusnitin";
            string serviceBusTokenIssuerName = "RootManageSharedAccessKey";
            string serviceBusTokenIssuerKey = "QSLCGZmsPZPcfxZGFhoWUxqkiCTgDQIV5P87gryqaLI=";
            string topicName = "topic1";
            ServiceBusFactory process = new ServiceBusFactory();
            IPublisher publisher = process.GetTopic(url, topicName, serviceBusTokenIssuerName, serviceBusTokenIssuerKey);
            Console.WriteLine("Press c to stop. Any other key to push a message");
            do
            {
                MessageFormat message = new MessageFormat() {Id=Guid.NewGuid().ToString(),Name = "Nitn", Age = 27 };

                await publisher.QueueMessage<MessageFormat>(message);
                Console.WriteLine("Pushed message "+JsonConvert.SerializeObject(message));
                keyInfo = Console.ReadKey(false);
            } while (keyInfo.Key != ConsoleKey.C);
            Console.WriteLine("Stopping publisher");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            
        }


    }
}
