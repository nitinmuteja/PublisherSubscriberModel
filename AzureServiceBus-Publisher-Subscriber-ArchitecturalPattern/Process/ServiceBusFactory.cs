using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Process.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process
{
    public class ServiceBusFactory
    {
        public IPublisher GetTopic(string serviceBusName, string topicName,
            string serviceBusTokenKeyName,
        string serviceBusToken)
        {
            return new AzurePublisher(serviceBusName, topicName, serviceBusTokenKeyName, serviceBusToken);
        }

        public ISubscriber GetSubscription(string serviceBusName, string topicName, string subscriptionName,
            string serviceBusTokenKeyName,
        string serviceBusToken,
       ReceiveMode mode,
       int? prefetchCount = null)
        {
            return new AzureSubscriber(serviceBusName, topicName, subscriptionName, serviceBusTokenKeyName, serviceBusToken, mode, prefetchCount);

        }

    }
}
