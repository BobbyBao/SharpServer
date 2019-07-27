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
        static ISiloHost siloHost;

        static Task Main(string[] args)
        {
            //new MasterServer(args).Start();

            return new HostBuilder()
                .UseOrleans(builder =>
                {
                    builder
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "HelloWorldApp";
                        })
                        .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(Player).Assembly).WithReferences());
                })
                .ConfigureServices(services =>
                {
                    services.Configure<ConsoleLifetimeOptions>(options =>
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
