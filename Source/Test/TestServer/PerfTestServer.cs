namespace Test.Server
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using ServiceStack;
    using ServiceStack.Text;
    using SharpServer;

    /// <summary>
    /// 服务端处理事件函数
    /// </summary>
    public class PerfTestServerHandler : SharpServer.NetworkServer.MessageHandler
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Interlocked.Increment(ref Stats.recv);
            context.WriteAsync(message);
            Interlocked.Increment(ref Stats.send);
        }

    }

    public class RerfTestServer : ServerApp<PerfTestServerHandler>
    {
        protected override void OnRun()
        {
            Task.Run(() => Server.Start<PerfTestServerHandler>(Port));

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
