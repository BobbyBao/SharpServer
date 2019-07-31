#define LIBUV
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;

namespace SharpServer
{
    public class NetworkServer
    {
        static IEventLoopGroup bossGroup;
        static IEventLoopGroup workerGroup;
        
        public IChannelGroup group;
        public ConcurrentDictionary<string, IChannelHandlerContext> channelHandlerContexts = new ConcurrentDictionary<string, IChannelHandlerContext>();

        public static void Init()
        {
#if LIBUV
            var dispatcher = new DispatcherEventLoopGroup();
            bossGroup = dispatcher;
            workerGroup = new WorkerEventLoopGroup(dispatcher);
#else
            bossGroup = new MultithreadEventLoopGroup(1);
            workerGroup = new MultithreadEventLoopGroup();
#endif
        }

        public static async Task Shutdown()
        {
            await Task.WhenAll(
                bossGroup?.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                workerGroup?.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
        }

        IChannel boundChannel;
        public async Task Start(int port, Action<IChannel> initializer)
        {
            try
            {
                var serverBootstrap = new ServerBootstrap();
                serverBootstrap.Group(bossGroup, workerGroup);
#if LIBUV
                serverBootstrap.Channel<TcpServerChannel>();
#else
                serverBootstrap.Channel<TcpServerSocketChannel>();
#endif
                serverBootstrap
                    .Option(ChannelOption.SoBacklog, 1024)
                    .Handler(new LoggingHandler("SRV-LSTN"))                    
                    .ChildHandler(new ActionChannelInitializer<IChannel>(initializer));

                boundChannel = await serverBootstrap.BindAsync(port);
                Log.Info($"Listen port : {port}");
            }
            catch(Exception e)
            {
                Log.Info(e, "Exception start server on port : " + port);
            }
            
            finally
            {
//                 await Task.WhenAll( 
//                     bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
//                     workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1))
//                 );
            }
        }

        public async Task<int> Broadcast(IByteBuffer byteBuffer)
        {
            await group.WriteAndFlushAsync(byteBuffer);
            return group.Count;
        }


        void ChannelRegistered(IChannelHandlerContext context)
        {
            IChannelGroup g = group;
            if (g == null)
            {
                lock (this)
                {
                    if (group == null)
                    {
                        g = group = new DefaultChannelGroup(context.Executor);
                    }
                }
            }
            var id = context.Channel.Id.AsLongText();
            channelHandlerContexts.TryAdd(id, context);
            group.Add(context.Channel);
            Log.Info("ChannelRegistered: " + id);
        }

        void ChannelUnregistered(IChannelHandlerContext context)
        {
            var id = context.Channel.Id.AsLongText();
            Log.Info("ChannelUnregistered: " + id);
            group.Remove(context.Channel);
            channelHandlerContexts.TryRemove(id, out IChannelHandlerContext channelHandlerContext);

        }

        public async Task Close()
        {
            await boundChannel?.CloseAsync();
        }
    }
}
