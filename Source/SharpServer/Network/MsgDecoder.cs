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
        public int LengthOffset { get; } = 0;
        public int HeadSize { get; } = 8;

        public MsgDecoder(int lenOffset = 0, int headSize = 8)
        {
            LengthOffset = lenOffset;
            HeadSize = headSize;
        }

        protected override void Decode(IChannelHandlerContext channelHandlerContext, IByteBuffer byteBuf, List<Object> list)
        {
            if (byteBuf.ReadableBytes < HeadSize)
            {
                return;
            }

            int length = byteBuf.GetInt(byteBuf.ReaderIndex + LengthOffset); // 获取取消息内容长度
            if (byteBuf.ReadableBytes < length)
            { 
                return;
            }

            int readerIndex = byteBuf.ReaderIndex;

            IByteBuffer frame = byteBuf.Slice(readerIndex, length);
            frame.Retain();
            byteBuf.SetReaderIndex(readerIndex + length);
            
            list.Add(frame);
        }


    }
}
