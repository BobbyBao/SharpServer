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
        static MultithreadEventLoopGroup group;
        static Bootstrap bootstrap;
        static List<IChannel> clientChannels = new List<IChannel>();
        
        static async Task RunClientAsync()
        {
            group = new MultithreadEventLoopGroup();

            try
            {
                bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)                    
                    .Handler(
                        new ActionChannelInitializer<ISocketChannel>(
                            channel =>
                            {
                                IChannelPipeline pipeline = channel.Pipeline;
                                pipeline.AddLast(new LoggingHandler());
                                pipeline.AddLast("framing-enc", new LengthFieldPrepender(4));
                                pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));

                                //pipeline.AddLast("echo", new EchoClientHandler());
                            }))
                            ;



                for (int i = 0; i < 3000; i++ )
                {
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                    Task.Run(Connect);
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

                foreach (var clientChannel in clientChannels)
                await clientChannel.CloseAsync();
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }

        static async Task Connect()
        {
            while(true)
            {
                try
                {
                    IChannel clientChannel = await bootstrap.ConnectAsync(IPAddress.Parse("127.0.0.1"), 2239);
#if TEST_PINGPONG
                    clientChannel.Pipeline.AddLast("echo", new PingPongClientHandler());
#elif TEST_PERF
                    
                    clientChannel.Pipeline.AddLast("echo", new PerfTestClientHandler());
#else                    
                    clientChannel.Pipeline.AddLast("echo", new EchoClientHandler());
#endif


                    clientChannels.Add(clientChannel);

                    //IByteBuffer initialMessage = Unpooled.Buffer(1024);
                    //initialMessage.WriteBytes(Stats.testMsg);

                    break;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);

                    await Task.Delay(5000);
                }
            }


        }

        static void Main() => RunClientAsync().Wait();
    }
}