using GrainCollection;
using GrainInterfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterServer
{
    public class MasterServer : IHostedService
    {
        private readonly IGrainFactory factory;
        private readonly IClusterClient client;
        private readonly IHostApplicationLifetime lifetime;

        public MasterServer(IGrainFactory factory, IClusterClient client, IHostApplicationLifetime lifetime)
        {
            this.factory = factory;
            this.client = client;
            this.lifetime = lifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Info("MasterServer start");

            IGateMaster player = factory.GetGrain<IGateMaster>(0);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Info("MasterServer Stop");
            return Task.CompletedTask;
        }
    }
}
