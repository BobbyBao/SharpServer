using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;

namespace SharpServer
{
    public class SiloApp : AppBase
    {
        ISiloHost silo;
        Type[] types;
        public SiloApp(params Type[] types)
        {
            this.types = types;
        }

        protected override Task OnInit()
        {
            var builder = new SiloHostBuilder()
            .UseLocalhostClustering()
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "AdventureApp";
            })
            .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback);

            foreach (var t in types)
            {
                builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(t.Assembly).WithReferences());
            }

            builder.ConfigureLogging(logging => logging.AddConsole());
            silo = builder.Build();
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
