using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpServer
{
    public class ProtoUtil
    {
        public static byte[] Serialize<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] msg)
        {
            using (MemoryStream ms = new MemoryStream(msg))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }

    }
}
