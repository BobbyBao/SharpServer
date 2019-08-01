using GrainInterfaces;
using Microsoft.Extensions.Hosting;
using Orleans;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleServer
{
    public class BattleServer : IHostedService
    {
        private readonly IGrainFactory factory;
        private readonly IClusterClient client;
        private readonly IHostApplicationLifetime lifetime;

        public BattleServer(IGrainFactory factory, IClusterClient client, IHostApplicationLifetime lifetime)
        {
            this.factory = factory;
            this.client = client;
            this.lifetime = lifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Info("BattleServer start");

            IGateBattle player = factory.GetGrain<IGateBattle>(0);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Info("BattleServer Stop");
            return Task.CompletedTask;
        }
    }
}
