using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Interfaces.AzureClients
{
    public interface IServiceBusClient
    {
        public ServiceBusSender CreateSender();
    }
}
