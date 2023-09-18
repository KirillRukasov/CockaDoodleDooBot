using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Gateway.AzureClients;
using Gateway.Interfaces.AzureClients;
using Gateway.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(Gateway.Startup))]
namespace Gateway
{
    public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            _functionConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<ServiceBusOptions>(_functionConfig.GetSection("ServiceBusOptions"));

            builder.Services.AddAzureClients(clientBuilder =>
            {
                var provider = builder.Services.BuildServiceProvider();

                clientBuilder.UseCredential(new DefaultAzureCredential());
                clientBuilder.AddServiceBusClientWithNamespace(provider.GetRequiredService<IOptions<ServiceBusOptions>>().Value.FullyQualifiedNamespace);
            });

            builder.Services.AddTransient<IServiceBusClient, ServiceBusAllMessagesClient>();

            builder.Services.AddLogging();
        }
    }
}
