namespace Test.Server
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
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

            var sw = new Stopwatch();
            sw.Start();

            int lastRecv = 0;
            int lastSend = 0;
            int count = 0;
            while (true)
            {
                if(count < 1000000)
                {
                    Task.Run(async () =>
                    {
                        IByteBuffer initialMessage = Unpooled.Buffer(128);
                        initialMessage.WriteBytes(Stats.testMsg);
                        count += await Server.Broadcast(initialMessage);
                    });
                }

                if (sw.ElapsedMilliseconds >= 1000)
                {
                    sw.Restart();

                    Console.WriteLine("Send {0}, Receive {1} per sec", (int)(Stats.send - lastSend), (int)(Stats.recv - lastRecv));
                    lastRecv = Stats.recv;
                    lastSend = Stats.send;
                }

                Thread.Sleep(1);
            }
        }
    }
}
