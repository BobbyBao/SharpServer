using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpServer
{
    public interface IMsgHeader
    {
        int MsgLen { get; }
        int MsgType { get; }
    }

    public struct MsgHead : IMsgHeader
    {
        public int len;
        public int msgType;

        public int MsgLen => len;
        public int MsgType => msgType;
    }



    public struct MsgPacket
    {
        public const int MSG_SIZE_SIZE = 4;
        public const int MSG_TYPE_SIZE = 4;
        public const int HEADER_SIZE = MSG_SIZE_SIZE + MSG_TYPE_SIZE;

        public byte[] data;

        public ref int Len => ref Unsafe.As<byte, int>(ref data[0]);
        public ref int MsgID => ref Unsafe.As<byte, int>(ref data[MSG_TYPE_SIZE]);

        public MsgPacket(byte[] data)
        {
            this.data = data;
        }

        public MsgPacket(IByteBuffer byteBuf)
        {
            int len = byteBuf.GetInt(0);
            this.data = new byte[len];
            byteBuf.ReadBytes(this.data);
        }

        public MsgPacket(int msgType, byte[] body)
        {
            data = null;

            Encode(msgType, body, ref data);
        }

        public static void Encode(int msgType, byte[] body, ref byte[] msg)
        {
            int msgSize = body.Length + MsgPacket.HEADER_SIZE;
            if (msg == null || msg.Length != msgSize)
            {
                msg = new byte[msgSize];
            }

            msg.Write(0, msg.Length + MsgPacket.HEADER_SIZE);
            msg.Write(MSG_SIZE_SIZE, msgType);

            Array.Copy(body, 0, msg, HEADER_SIZE, body.Length);
        }

    }
}
