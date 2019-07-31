using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SharpServer
{
    public class SiloApp : AppBase
    {
        ISiloHost silo;
        protected override Task OnInit()
        {
            silo = new SiloHostBuilder()
            .UseLocalhostClustering()
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "AdventureApp";
            })
            .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
            //.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(RoomGrain).Assembly).WithReferences())
            .ConfigureLogging(logging => logging.AddConsole())
            .Build();
            return Task.CompletedTask;
        }

        protected override Task OnStart()
        {            
            return Task.CompletedTask;
        }

        protected override Task OnRun()
        {
            return Task.CompletedTask;
        }

        protected override Task OnShutdown()
        {
            return Task.CompletedTask;
        }

    }
}
