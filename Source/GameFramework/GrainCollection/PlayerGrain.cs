using MasterServer;
using GrainInterfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.IO;
using ProtoBuf;
using System.Runtime.CompilerServices;

namespace GrainCollection
{
    public class PlayerGrain : Orleans.Grain, IPlayerGrain
    {
        public Task<bool> Login(string name)
        {
            return Task.FromResult(true);
        }

        public Task<byte[]> SendMessage(int msgType, byte[] msg)
        {
            switch (msgType)
            {
                case (int)MessageType.UserLoginReq:
                    using (MemoryStream msRead = new MemoryStream(msg))
                    {
                        var req = Serializer.Deserialize<UserLoginReq>(msRead);

                        var res = new UserLoginRes
                        {
                            Res = 0,
                            UserId = "Test user id."
                        };

                        using (MemoryStream ms = new MemoryStream())
                        {
                            BinaryWriter bw = new BinaryWriter(ms);
                            bw.Write((int)ms.Position + 8);                           
                            bw.Write((int)MessageType.UserLoginRes);
                            ProtoBuf.Serializer.Serialize(ms, res);
                            var data = ms.ToArray();
                            Unsafe.As<byte, int>(ref data[0]) = (int)ms.Position + 8;
                            return Task.FromResult(data);
                        }
                    }

            }


            return null;
        }
    }
}
