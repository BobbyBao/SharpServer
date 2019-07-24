using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestMasterServer
{
    public class MasterClient : ClientApp
    {
        public MasterClient()
        {
        }

        protected override void OnRun()
        {
            for (int i = 0; i < 3000; i++)
            {
                Task.Run(Connect);
            }

            while (true)
            {
                Thread.Sleep(1000);
            }

        }
    }
}
