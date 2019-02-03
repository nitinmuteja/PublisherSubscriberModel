using Microsoft.ServiceBus.Messaging;
using Process.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process
{
    class AzureSubscriber:ISubscriber
    {
        private readonly SubscriptionClient _subscriptionClient;
        public AzureSubscriber(string serviceBusName, string topicName, string subscriptionName,
            string serviceBusTokenKeyName,
        string serviceBusToken,
       ReceiveMode mode,
       int? prefetchCount = null)
        {
            var namespaceManager = AzureFunctions.CreateNamespaceManager(serviceBusName,
                   serviceBusTokenKeyName, serviceBusToken
                   );

            MessagingFactorySettings messagingFactorySettings = new MessagingFactorySettings();
            messagingFactorySettings.TransportType = TransportType.NetMessaging;
            messagingFactorySettings.OperationTimeout = TimeSpan.FromMinutes(60);
            messagingFactorySettings.TokenProvider = namespaceManager.Settings.TokenProvider;


            MessagingFactory messagingFactory = MessagingFactory.Create(namespaceManager.Address, messagingFactorySettings);
            _subscriptionClient = messagingFactory.CreateSubscriptionClient(topicName, subscriptionName, mode);
            if (prefetchCount.HasValue)
                _subscriptionClient.PrefetchCount = prefetchCount.Value;
        }

        public void OnMessage<T>(Action<T> anonymousMethod)
        {
            _subscriptionClient.OnMessage((message) => {
                var messageFormat = message.GetBody<T>();
                anonymousMethod(messageFormat);
            },new OnMessageOptions() { MaxConcurrentCalls=100});
        }
    }
}
