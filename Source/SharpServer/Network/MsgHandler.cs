using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class MsgHandler : SimpleChannelInboundHandler<Object>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, Object obj)
        {
            //MsgPacket msg = (MsgPacket)obj;
        }

    }

}
