using Microsoft.ServiceBus.Messaging;
using Process.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process
{
    class AzurePublisher:IPublisher
    {
        private readonly TopicClient _topicClient;
       public AzurePublisher(string serviceBusName, string topicName,
            string serviceBusTokenKeyName,
        string serviceBusToken)
        {

            var namespaceManager = AzureFunctions.CreateNamespaceManager(serviceBusName,
             serviceBusTokenKeyName, serviceBusToken
             );

            //Create the queue if it does not exist already
            if (!namespaceManager.TopicExists(topicName))
            {
                var topicDescription = new TopicDescription(topicName)
                {
                    MaxSizeInMegabytes = 5120,
                    EnablePartitioning = true,
                    EnableBatchedOperations = true
                };
                namespaceManager.CreateTopic(topicDescription);
            }


            var messagingFactorySettings =
                new MessagingFactorySettings
                {
                    OperationTimeout = TimeSpan.FromMinutes(60),
                    TransportType = Microsoft.ServiceBus.Messaging.TransportType.Amqp,
                    TokenProvider = namespaceManager.Settings.TokenProvider
                };
            var messagingFactory = MessagingFactory.Create(namespaceManager.Address, messagingFactorySettings);
            _topicClient = messagingFactory.CreateTopicClient(topicName);
        }

        /// <summary>
        /// Queues a message in servicebus
        /// </summary>
        /// <typeparam name="T">Message type to queue</typeparam>
        /// <param name="message">Message instance to be queued</param>
        /// <param name="scheduleTime">Nullable value. Time when the message should move to a subscriber</param>
        /// <returns></returns>
        public async Task QueueMessage<T>(T message, DateTime? scheduleTime=null)
        {
            BrokeredMessage brokeredMessage = new BrokeredMessage(message);
            if(scheduleTime.HasValue)
            brokeredMessage.ScheduledEnqueueTimeUtc = scheduleTime.Value;

            brokeredMessage.Properties["MessageType"] = "MessageType1";
            await _topicClient.SendAsync(brokeredMessage);

        }
    }
}
