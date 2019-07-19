//#define TEST_PINGPONG
#define TEST_PERF

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using SharpServer;

namespace Test.Server
{
    class Program
    {
        static void Main()
        {
            const int port = 2239;

            var synchronizationContext = SynchronizationContext.Current;
            var unityThreadId = Thread.CurrentThread.ManagedThreadId;
#if TEST_PINGPONG              
            new SharpServer.ServerApp().Run<PingPongServer>(port).Wait();
#elif TEST_PERF
            var svr = new SharpServer.ServerApp();
            Task.Run(()=> svr.Run<PerfTestServer>(port));

            var sw = new Stopwatch();
            sw.Start();

            int lastRecv = 0;
            int lastSend = 0;
            while (true)
            {
                Task.Run(async () =>
                {
                    IByteBuffer initialMessage = Unpooled.Buffer(128);
                    initialMessage.WriteBytes(Stats.testMsg);
                    //for(int i = 0; i < 100; i++)
                    await svr.Server.Broadcast(initialMessage);
                });

                if(sw.ElapsedMilliseconds >= 1000)
                {
                    sw.Restart();

                    Console.WriteLine("Send {0}, Receive {1} per sec", (int)(Stats.send - lastSend), (int)(Stats.recv - lastRecv));
                    lastRecv = Stats.recv;
                    lastSend = Stats.send;
                }

                Thread.Sleep(1);
            }
#else                    
            new SharpServer.ServerApp().Run<EchoServer>(port).Wait();
#endif
        }
    }
}
