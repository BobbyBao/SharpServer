using DotNetty.Transport.Channels;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterServer
{
    public class MasterServerHandler : SharpServer.ServerHandler
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            context.WriteAsync(message);

        }

    }

    public class MasterServer : ServerApp<MasterServerHandler>
    {
        protected override void OnRun()
        {
            DoListen();

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
