using DotNetty.Transport.Channels;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterServer
{
    public class MasterServer : ServerApp
    {
        Connection handler;
        public MasterServer()
        {
        }

        protected override void OnConnect(Connection handler)
        {
            this.handler = handler;

            handler.Register<UserLoginReq>((int)MessageType.UserLoginReq, HandleUserLoginReq);
        }

        void HandleUserLoginReq(UserLoginReq msg)
        {
            Log.Info("User {0}, login", msg.UserName);

            var res = new UserLoginRes
            {
                Res = 0
            };

            handler.Send((int)MessageType.UserLoginRes, res);
        }

    }
}
