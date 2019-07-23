using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpServer
{
    public struct MsgPacket
    {
        public const int MSG_SIZE_SIZE = 4;
        public const int MSG_TYPE_SIZE = 4;
        public const int HEADER_SIZE = MSG_SIZE_SIZE + MSG_TYPE_SIZE;

        public byte[] data;

        public int Len => Unsafe.As<byte, int>(ref data[0]);
        public int MsgID => Unsafe.As<byte, int>(ref data[MSG_TYPE_SIZE]);

        public MsgPacket(byte[] data)
        {
            this.data = data;
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
