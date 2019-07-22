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
    public class ServerHandler : BaseHandler
    {
        public NetworkServer server;

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            base.ChannelRegistered(context);

            IChannelGroup g = server.group;
            if (g == null)
            {
                lock (this)
                {
                    if (server.group == null)
                    {
                        g = server.group = new DefaultChannelGroup(context.Executor);
                    }
                }
            }
            var id = context.Channel.Id.AsLongText();
            server.channelHandlerContexts.TryAdd(id, context);
            server.group.Add(context.Channel);
            Console.WriteLine("ChannelRegistered|" + server.channelHandlerContexts.Count);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            Console.WriteLine("ChannelUnregistered|" + server.channelHandlerContexts.Count);
            server.group.Remove(context.Channel);
            server.channelHandlerContexts.TryRemove(context.Channel.Id.AsLongText(), out IChannelHandlerContext channelHandlerContext);

            base.ChannelUnregistered(context);
        }

    }
}
