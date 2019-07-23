namespace Test.Server
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using ServiceStack;
    using ServiceStack.Text;
    using SharpServer;
    
    public class PingPongServerHandler : ServerHandler
    {
        public PingPongServerHandler()
        {
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Interlocked.Increment(ref Stats.recv);
            context.WriteAsync(message);
            Interlocked.Increment(ref Stats.send);
        }

    }

    public class PingPongServer : ServerApp<PingPongServerHandler>
    {
        protected override void OnRun()
        {
            DoListen();

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
