using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public delegate void MsgProcessor(object args);

    public class MsgHandler : SimpleChannelInboundHandler<IByteBuffer>
    {
        IChannelHandlerContext context;
        ConcurrentDictionary<int, MsgProcessor> messageHandlers = new ConcurrentDictionary<int, MsgProcessor>();
        public MsgHandler(bool autoRelease = true) : base(autoRelease)
        {
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
            this.context = context;
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
            this.context = null;
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            base.ChannelRegistered(context);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            base.ChannelUnregistered(context);
        }

        protected override void ChannelRead0(IChannelHandlerContext context, IByteBuffer message)
        {
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
