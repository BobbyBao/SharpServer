﻿using Orleans.Hosting;
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

namespace BattleServer
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
                            options.ServiceId = "battle_Server";
                        })
                        .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(BattleGrain).Assembly).WithReferences());
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<BattleServer>()
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
