//#define TEST_PINGPONG
#define TEST_PERF


namespace Test.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Codecs;
    using DotNetty.Handlers.Logging;
    using DotNetty.Transport.Bootstrapping;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;
    using SharpServer;

    static class Program
    {
        static async Task RunClientAsync()
        {
            NetworkClient.Init();

            for (int i = 0; i < 3000; i++ )
            {
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Task.Run(()=> NetworkClient.Connect<PerfTestClientHandler>("127.0.0.1", 2239));
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            }


            int lastRecv = 0;
            int lastSend = 0;
            await Task.Run(async () =>
            {
                while (true)
                {
                    //    Console.ReadLine();
                    await Task.Delay(1000);

                    Console.WriteLine("Send {0}, Receive {1} per sec", (int)(Stats.send - lastSend), (int)(Stats.recv - lastRecv));
                    lastRecv = Stats.recv;
                    lastSend = Stats.send;
                }
            });

            //foreach (var clientChannel in clientChannels)
            //await clientChannel.CloseAsync();
//             }
//             finally
//             {
//                 await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
//             }
        }

        static void Main() => RunClientAsync().Wait();
    }
}
