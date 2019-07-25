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
            bootstrap.Group(group);
        }

        public static async void Shutdown()
        {
            foreach (var clientChannel in clientChannels)
                await clientChannel.CloseAsync();

            await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
        }

        public static async Task Connect(string ip, int port, Action<ISocketChannel> initializer)
        {
            while (true)
            {
                try
                {
                    Log.Info("Connecting {0}:{1}!", ip, port);
                    bootstrap
                   .Channel<TcpSocketChannel>()
                   .Option(ChannelOption.TcpNodelay, true)
                   .Handler(new ActionChannelInitializer<ISocketChannel>(initializer));

                    IChannel clientChannel = await bootstrap.ConnectAsync(IPAddress.Parse(ip), port);                    
                    clientChannels.Add(clientChannel);
                    //Log.Info("Connect succ!");
                    return;
                    
                }
                catch (Exception e)
                {
                    Log.Info(e.InnerException, "Connect failed!");

                    await Task.Delay(5000);
                }
            }

        }
    }
}
