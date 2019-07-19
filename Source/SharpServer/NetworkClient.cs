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
        public static void Init()
        {
            var group = new MultithreadEventLoopGroup();
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
        }

        public static async void Connect<T>(string ip, int port) where T : IChannelHandler, new()
        {
            IChannel clientChannel = await bootstrap.ConnectAsync(IPAddress.Parse(ip), port);
        }
    }
}
