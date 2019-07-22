using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer
{
    public class ServerHandler : BaseChannelHandler
    {
        public IChannelGroup group;
        public ConcurrentDictionary<string, IChannelHandlerContext> channelHandlerContexts = new ConcurrentDictionary<string, IChannelHandlerContext>();

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

        public async Task<int> Broadcast(IByteBuffer byteBuffer)
        {
            await group.WriteAndFlushAsync(byteBuffer);
            Interlocked.Add(ref Stats.send, group.Count);
            return group.Count;
        }
    }
}
