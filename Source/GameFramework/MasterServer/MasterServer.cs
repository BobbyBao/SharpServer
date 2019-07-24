using DotNetty.Transport.Channels;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterServer
{
    public class MasterServer : ServerApp
    {
        public MasterServer(string[] args) : base(args)
        {
        }

        protected override void OnRun()
        {
            Task.Run(Listen);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
