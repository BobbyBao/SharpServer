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
        public string channelID;
        public IChannelHandlerContext context;
        ConcurrentDictionary<int, MsgProcessor> msgHandlers = new ConcurrentDictionary<int, MsgProcessor>();

        public event Action<MsgHandler> connected;
        public event Action<MsgHandler> disconnected;

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

        public void Send<T>(int msgType, T obj)
        {
            Task.Run(async () =>
            {
                MemoryStream ms = new MemoryStream();
                ProtoBuf.Serializer.Serialize(ms, obj);
                int len = (int)ms.Position;
                IByteBuffer byteBuf = Unpooled.Buffer(len + 8);
                EncodeHead(byteBuf, msgType, len);
                byteBuf.WriteBytes(ms.ToArray());
                await context.WriteAndFlushAsync(byteBuf);
                //Log.Info("Send : {0}", len);
            });
        }

        public async Task Send(int msgType, byte[] body)
        {
            IByteBuffer message = Unpooled.Buffer(body.Length + 8);
            Encode(message, msgType, body);

            await context.WriteAndFlushAsync(message);
            //Log.Info("Send bytes: {0}", body.Length);
        }

        public void Register<T>(int msgType, Action<T> handler)
        {
            msgHandlers.TryAdd(msgType, (buf) =>
            {
                var msg = Deserialize<T>(buf.Array, buf.Offset, buf.Count);
                handler.Invoke(msg);
            });
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
            connected?.Invoke(this);
            this.context = context;
            //Log.Info("Channel connected: {0}", channelID);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);

            disconnected?.Invoke(this);
            //Log.Info("Channel disconnected: {0}", channelID);

            this.context = null;
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            base.ChannelRegistered(context);

            channelID = context.Channel.Id.AsShortText();

            //Log.Info("Channel registered: {0}", channelID);
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            base.ChannelUnregistered(context);

            //Log.Info("Channel unregistered: {0}", channelID);
            channelID = "";
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            //Log.Info("ChannelRead" + message.ToString());

            base.ChannelRead(context, message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Log.Info(exception, "");
            context.CloseAsync();
        }

        protected override void ChannelRead0(IChannelHandlerContext context, IByteBuffer message)
        {
            int msgType = message.GetInt(4);
            if (msgHandlers.TryGetValue(msgType, out var handler))
            {
                int msgLen = message.GetInt(0);
                handler.Invoke(message.GetIoBuffer(8, msgLen - 8));
            }
        }

    }
}
