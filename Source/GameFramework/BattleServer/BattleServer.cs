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
        private readonly IGrainFactory _factory;
        private readonly IClusterClient _client;
        private readonly IHostApplicationLifetime _lifetime;

        public BattleServer(IGrainFactory factory, IClusterClient client, IHostApplicationLifetime lifetime)
        {
            _factory = factory;
            _client = client;
            _lifetime = lifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Info("BattleServer start");

            IGateBattle player = _factory.GetGrain<IGateBattle>(0);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Info("BattleServer Stop");
            return Task.CompletedTask;
        }
    }
}
