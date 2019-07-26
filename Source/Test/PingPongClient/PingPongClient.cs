using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Client
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using SharpServer;

    public class PingPongClientHandler : Connection
    {
        public PingPongClientHandler()
        {
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);

            for (int j = 0; j < 200; j++)
            {
                Task.Run(async () =>
                {
                    await Send(0, Stats.testMsg);
                });
            }

        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Interlocked.Increment(ref Stats.recv);
            context.WriteAsync(message);
            Interlocked.Increment(ref Stats.send);
        }

    }

    public class PingPongClient : ClientApp
    {
        int lastRecv = 0;
        int lastSend = 0;
        public PingPongClient()
        {
        }

        protected override Connection CreateConnection()
        {
            return new PingPongClientHandler();
        }

        protected override void OnStart()
        {
            for (int i = 0; i < 3000; i++)
            {
                Task.Run(Connect);
            }
        }

        protected override void OnTick(int msec)
        {
            Log.Info("Send {0}, Receive {1} per sec", (int)(Stats.send - lastSend), (int)(Stats.recv - lastRecv));
            lastRecv = Stats.recv;
            lastSend = Stats.send;
        }
    }
}
