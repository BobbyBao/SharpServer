using MasterServer;
using GrainInterfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.IO;
using ProtoBuf;
using System.Runtime.CompilerServices;
using SharpServer;

namespace GrainCollection
{
    public class GateGrain : Orleans.Grain, IGateGrain
    {
        public IMessageProc MessageProc { get; set; }

        public override Task OnActivateAsync()
        {
            Log.Info("Grain active : " + this.IdentityString);
            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            Log.Info("Grain deactive : " + this.IdentityString);
            return base.OnDeactivateAsync();
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
                            ProtoBuf.Serializer.Serialize(ms, res);
                            return Task.FromResult(ms.ToArray());
                        }
                    }

            }


            return null;
        }
    }
}
