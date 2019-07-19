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
using DotNetty.Transport.Libuv;

namespace SharpServer
{
    public class NetworkServer
    {
        IEventLoopGroup bossGroup;
        IEventLoopGroup workerGroup;
        public async Task Start<T>(int port) where T : IChannelHandler, new() 
        {
            var dispatcher = new DispatcherEventLoopGroup();
            bossGroup = dispatcher;
            workerGroup = new WorkerEventLoopGroup(dispatcher);

            try
            {
                var serverBootstrap = new ServerBootstrap();
                serverBootstrap.Group(bossGroup, workerGroup);
                serverBootstrap.Channel<TcpServerChannel>();

                serverBootstrap
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler("SRV-LSTN"))                    
                    .ChildHandler(new ActionChannelInitializer<IChannel>(
                        channel =>
                        {
                            IChannelPipeline pipeline = channel.Pipeline;
                            pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                            pipeline.AddLast("framing-enc", new LengthFieldPrepender(4));
                            pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));
                            pipeline.AddLast("handler", new T());
                        }));

                IChannel boundChannel = await serverBootstrap.BindAsync(port);

                Console.WriteLine("wait the client input");
                Console.ReadLine();

                await boundChannel.CloseAsync();
            }
            finally
            {
                // 释放指定工作组线程
                await Task.WhenAll( // (7)
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1))
                );
            }
        }

        public async void Shutdown()
        {
            await Task.WhenAll(
                bossGroup?.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),               
                workerGroup?.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
        }

        public static IChannelGroup group;
        public static ConcurrentDictionary<string, IChannelHandlerContext> channelHandlerContexts = new ConcurrentDictionary<string, IChannelHandlerContext>();
        public async Task<int> Broadcast(IByteBuffer byteBuffer)
        {
            if(group != null)
            {
                await group.WriteAndFlushAsync(byteBuffer);                
                Interlocked.Add(ref Stats.send, group.Count);
                return group.Count;
            }

            return 0;
        }

        public class MessageHandler : ChannelHandlerAdapter
        {
            public override void ChannelRegistered(IChannelHandlerContext context)
            {
                base.ChannelRegistered(context);

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
                Console.WriteLine("ChannelRegistered|" + channelHandlerContexts.Count);
            }

            public override void ChannelUnregistered(IChannelHandlerContext context)
            {
                Console.WriteLine("ChannelUnregistered|" + channelHandlerContexts.Count);
                group.Remove(context.Channel);
                channelHandlerContexts.TryRemove(context.Channel.Id.AsLongText(), out IChannelHandlerContext channelHandlerContext);

                base.ChannelUnregistered(context);
            }

            public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

            public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
            {
                Console.WriteLine("Exception: " + exception);
                context.CloseAsync();
            }
        }
    }
}
