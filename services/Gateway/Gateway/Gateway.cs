using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using Telegram.Bot.Types;
using Gateway.Extensions;
using Gateway.Interfaces.AzureClients;
using Azure.Messaging.ServiceBus;

namespace Gateway
{
    public class Gateway
    {
        private readonly IServiceBusClient _serviceBusClient;

        public Gateway(IServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        [FunctionName("Gateway")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Update tgUpdate,
            ILogger log)
        {
            string messageString = tgUpdate.ToJson();
            log.LogInformation($"Update received: {messageString}");

            return await SendMessageToServiceBus(log, messageString);
        }

        private async Task<IActionResult> SendMessageToServiceBus(ILogger log, string messageString)
        {
            try
            {
                await using ServiceBusSender sender = _serviceBusClient.CreateSender();
                ServiceBusMessage serviceBusMessage = new(messageString);
                await sender.SendMessageAsync(serviceBusMessage);

                return new OkResult();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error sending to service bus: {0}", messageString);
                return new BadRequestObjectResult("Something went wrong, try again later");
            }
        }
    }
}
