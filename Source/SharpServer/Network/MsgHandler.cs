using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public delegate void MsgProcessor(ArraySegment<byte> buf);

    public class MsgHandler : SimpleChannelInboundHandler<IByteBuffer>
    {
        IChannelHandlerContext context;
        ConcurrentDictionary<int, MsgProcessor> messageHandlers = new ConcurrentDictionary<int, MsgProcessor>();

        public event Action<IChannelHandlerContext> channelRegistered;
        public event Action<IChannelHandlerContext> channelUnregistered;

        public MsgHandler(bool autoRelease = true) : base(autoRelease)
        {
        }

        protected virtual void EncodeHead(IByteBuffer message, int msgType, int bodyLen)
        {
            message.WriteInt(bodyLen + 8);
            message.WriteInt(msgType);
        }

        protected void Encode(IByteBuffer message, int msgType, byte[] body)
        {
            EncodeHead(message, msgType, body.Length);
            message.WriteBytes(body);
        }

        public T Deserialize<T>(byte[] byteBuf, int offset, int size)
        {
            MemoryStream ms = new MemoryStream(byteBuf, offset, size);
            return ProtoBuf.Serializer.Deserialize<T>(ms);
        }

        public async Task Send<T>(int msgType, T obj)
        {
            MemoryStream ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, obj);
            int len = (int)ms.Position;
            IByteBuffer byteBuf = Unpooled.Buffer(len + 8);
            EncodeHead(byteBuf, msgType, len);
            await byteBuf.WriteBytesAsync(ms, len);
        }

        public async Task Send(int msgType, byte[] body)
        {
            IByteBuffer message = Unpooled.Buffer(body.Length + 8);
            Encode(message, msgType, body);

            await context.WriteAndFlushAsync(message);
        }

        public void Register<T>(int msgType, Action<T> handler)
        {
            messageHandlers.TryAdd(msgType, (buf) =>
            {
                var msg = Deserialize<T>(buf.Array, buf.Offset, buf.Count);
                handler.Invoke(msg);
            });
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

            channelRegistered?.Invoke(context);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            channelUnregistered?.Invoke(context);

            base.ChannelUnregistered(context);
        }

        protected override void ChannelRead0(IChannelHandlerContext context, IByteBuffer message)
        {
            int msgType = message.GetInt(4);
            if(messageHandlers.TryGetValue(msgType, out var handler))
            {
                int msgLen = message.GetInt(0);
                handler.Invoke(message.GetIoBuffer(8, msgLen - 8));
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}
