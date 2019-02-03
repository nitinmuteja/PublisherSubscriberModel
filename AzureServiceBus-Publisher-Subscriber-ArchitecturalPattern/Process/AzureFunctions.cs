using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process
{
    class AzureFunctions
    {

        public static NamespaceManager CreateNamespaceManager(string url, string issuerName, string issuerKey)
        {
            // Create the namespace manager which gives you access to
            // management operations
            var uri = ServiceBusEnvironment.CreateServiceUri(
                "sb", url, string.Empty);
            var tP = TokenProvider.CreateSharedAccessSignatureTokenProvider(
                issuerName, issuerKey);
            return new NamespaceManager(uri, tP);
        }

    }
}
