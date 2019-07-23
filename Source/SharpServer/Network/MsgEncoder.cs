using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class MsgEncoder : MessageToByteEncoder<byte[]>
    {
        protected override void Encode(IChannelHandlerContext context, byte[] message, IByteBuffer output)
        {
            output.WriteBytes(message);
        }

    }
}
