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
    public class MasterGrain : Orleans.Grain, IGateMaster
    {
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
                    {
                        var req = ProtoUtil.Deserialize<UserLoginReq>(msg);

                        var res = new UserLoginRes
                        {
                            Res = 0,
                            UserId = Guid.NewGuid().ToByteArray()
                        };

                        var ret = ProtoUtil.Serialize(res);
                        return Task.FromResult(ret);
                    }
            }


            return null;
        }
    }
}
