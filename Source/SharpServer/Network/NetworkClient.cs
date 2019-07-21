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

namespace SharpServer
{
    public class NetworkClient
    {
        static Bootstrap bootstrap = new Bootstrap();
        static IEventLoopGroup group;
        static List<IChannel> clientChannels = new List<IChannel>();
        public static void Init()
        {
            group = new MultithreadEventLoopGroup();
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

                        }))
                        ;
        }

        public static async void Shutdown()
        {
            foreach (var clientChannel in clientChannels)
                await clientChannel.CloseAsync();

            await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
        }

        public static async Task<IChannel> Connect<T>(string ip, int port) where T : IChannelHandler, new()
        {
            while (true)
            {
                try
                {
                    IChannel clientChannel = await bootstrap.ConnectAsync(IPAddress.Parse(ip), 2239);          
                    clientChannel.Pipeline.AddLast("echo", new T());
                    clientChannels.Add(clientChannel);

                    return clientChannel;
                    
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);

                    if(e.InnerException != null)
                    {
                        Console.Write(" -- ");
                        Console.WriteLine(e.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                    await Task.Delay(5000);
                }
            }

        }
    }
}
