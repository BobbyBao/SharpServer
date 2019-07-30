using GrainCollection;
using GrainInterfaces;
using Microsoft.Extensions.Hosting;
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
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Info("MasterServer start");

            //GateGrain player = GrainFactory.GetGrain<IGateGrain>(1);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Info("MasterServer Stop");
            return Task.CompletedTask;
        }
    }
}
