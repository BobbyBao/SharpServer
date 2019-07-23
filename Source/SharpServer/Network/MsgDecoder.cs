using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class MsgDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext channelHandlerContext, IByteBuffer byteBuf, List<Object> list)
        {
            if (byteBuf.ReadableBytes < MsgPacket.HEADER_SIZE)
            {
                return;
            }

            int length = byteBuf.GetInt(byteBuf.ReaderIndex); // 获取取消息内容长度
            if (byteBuf.ReadableBytes < length)
            { 
                return;
            }

            int readerIndex = byteBuf.ReaderIndex;
            IByteBuffer frame = ExtractFrame(byteBuf, readerIndex, length);
            byteBuf.SetReaderIndex(readerIndex + length);
            
            list.Add(frame);
        }

        protected IByteBuffer ExtractFrame(IByteBuffer buffer, int index, int length)
        {
            IByteBuffer buff = buffer.Slice(index, length);
            buff.Retain();
            return buff;
        }
    }
}
