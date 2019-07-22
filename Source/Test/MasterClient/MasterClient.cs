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
    public class MasterClientHandler : BaseHandler
    {
        public MasterClientHandler()
        {
        }

    }

    public class MasterClient : ClientApp<MasterClientHandler>
    {
        public MasterClient()
        {
        }

        protected override void OnRun()
        {
            for (int i = 0; i < 3000; i++)
            {
                DoConnect();
            }

            int lastRecv = 0;
            int lastSend = 0;

            while (true)
            {
                Thread.Sleep(1000);

                Console.WriteLine("Send {0}, Receive {1} per sec", (int)(Stats.send - lastSend), (int)(Stats.recv - lastRecv));
                lastRecv = Stats.recv;
                lastSend = Stats.send;
            }

        }
    }
}
