using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Client
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using SharpServer;

    public class PerfTestClientHandler : ChannelHandlerAdapter
    {
        public PerfTestClientHandler()
        {
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Interlocked.Increment(ref Stats.recv);
            context.WriteAsync(message);
            Interlocked.Increment(ref Stats.send);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }

    public class PerfTestClient : ClientApp<PerfTestClientHandler>
    {
        protected override void OnRun()
        {
            for (int i = 0; i < 3000; i++)
            {
                Task.Run(() => NetworkClient.Connect<PerfTestClientHandler>("127.0.0.1", 2239));
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
