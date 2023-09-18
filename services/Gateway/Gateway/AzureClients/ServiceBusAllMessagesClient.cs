using System;
using Azure.Messaging.ServiceBus;
using Gateway.Interfaces.AzureClients;
using Gateway.Options;
using Microsoft.Extensions.Options;

namespace Gateway.AzureClients
{
	public class ServiceBusAllMessagesClient : IServiceBusClient
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;

        public ServiceBusAllMessagesClient(ServiceBusClient serviceBusClient, IOptions<ServiceBusOptions> options)
		{
            _serviceBusClient = serviceBusClient;
            _serviceBusOptions = options.Value;
        }

        public ServiceBusSender CreateSender()
            => _serviceBusClient.CreateSender(_serviceBusOptions.AllMessagesTopic);
    }
}

