using Models;
using Newtonsoft.Json;
using Process;
using Process.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusSubscriber2
{
    class Program
    {
        static void Main(string[] args)
        {

            ServiceBusFactory process = new ServiceBusFactory();
            string url = "testservicebusnitin";
            string subscriptionName = "subs2";
            string serviceBusTokenIssuerName = "RootManageSharedAccessKey";
            string serviceBusTokenIssuerKey = "QSLCGZmsPZPcfxZGFhoWUxqkiCTgDQIV5P87gryqaLI=";
            string topicName = "topic1";

            ISubscriber subscriber = process.GetSubscription(url, topicName, subscriptionName, serviceBusTokenIssuerName, serviceBusTokenIssuerKey, Microsoft.ServiceBus.Messaging.ReceiveMode.ReceiveAndDelete, 100);
            subscriber.OnMessage<MessageFormat>((message) => { Console.WriteLine(JsonConvert.SerializeObject(message)); });
            Console.ReadKey();
        }
    }
}
