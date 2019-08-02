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
using System.Collections.Generic;

namespace GrainCollection
{
    public class MasterGrain : Orleans.Grain, IGateMaster
    {
        Dictionary<MessageType, MessageHandler> messageHandlers = new Dictionary<MessageType, MessageHandler>();

        public void Register(MessageType type, MessageHandler handler)
        {
            messageHandlers[type] = handler;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

            Log.Info("Grain active : " + this.IdentityString);

            Register(MessageType.UserLoginReq, HandleUserLoginReq);
        }

        public override async Task OnDeactivateAsync()
        {
            messageHandlers.Clear();

            Log.Info("Grain deactive : " + this.IdentityString);

            await base.OnDeactivateAsync();
        }

        public async Task<byte[]> SendMessage(int msgType, byte[] msg)
        {

            if(messageHandlers.TryGetValue((MessageType)msgType, out var handler))
            {
                await handler(msg);
            }
            else
            {
                Log.Info("Message not handler : {0}" + (MessageType)msgType);
            }


            return null;
        }

        async Task<byte[]> HandleUserLoginReq(byte[] msg)
        {
            var req = ProtoUtil.Deserialize<UserLoginReq>(msg);

            //todo 

            var res = new UserLoginRes
            {
                Res = 0,
                UserId = Guid.NewGuid().ToByteArray()
            };

            return ProtoUtil.Serialize(res);
        }
    }
}
