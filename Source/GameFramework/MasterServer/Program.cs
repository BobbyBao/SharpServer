using Orleans.Hosting;
using Orleans.Runtime;
using Orleans;
using System;
using GrainInterfaces;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using System.Net;
using GrainCollection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MasterServer
{
    class Program
    {
        static Task Main(string[] args)
        {
            return new HostBuilder()
                .UseOrleans(builder =>
                {
                    builder
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "master_Server";
                        })
                        .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(MasterGrain).Assembly).WithReferences())
                        .AddStartupTask(async (provider, token) =>
                         {
                             var factory = provider.GetService<IGrainFactory>();
                             var client = provider.GetService<IClusterClient>();

                             // make the first producer grain change every five seconds
                             var grain = factory.GetGrain<IGateMaster>(0);

                             // make the second producer grain change every fifteen seconds
                             //await factory.GetGrain<IGateGrain>("B").StartAsync(10, TimeSpan.FromSeconds(15));
                         });
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<MasterServer>()
                    .Configure<ConsoleLifetimeOptions>(options =>
                    {
                        options.SuppressStatusMessages = true;
                    });
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                })
                .RunConsoleAsync();
        }
    }

    
}
