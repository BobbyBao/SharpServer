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
        private readonly IGrainFactory _factory;
        private readonly IClusterClient _client;
        private readonly IApplicationLifetime _lifetime;

        public MasterServer(IGrainFactory factory, IClusterClient client, IApplicationLifetime lifetime)
        {
            _factory = factory;
            _client = client;
            _lifetime = lifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Info("MasterServer start");

            IGateMaster player = _factory.GetGrain<IGateMaster>(0);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Info("MasterServer Stop");
            return Task.CompletedTask;
        }
    }
}
