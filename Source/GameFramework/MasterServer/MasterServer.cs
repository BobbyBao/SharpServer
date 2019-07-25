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
        public MasterServer(string[] args) : base(args)
        {
        }

        Connection handler;

        protected override void OnConnect(Connection handler)
        {
            this.handler = handler;

            handler.Register<UserLoginReqT>((int)MessageType.UserLoginReq, HandleUserLoginReq);
        }

        void HandleUserLoginReq(UserLoginReqT msg)
        {
            Log.Info("User {0}, login", msg.UserName);

            var res = new UserLoginResT
            {
                Res = 0
            };

            handler.Send((int)MessageType.UserLoginRes, res);
        }

        protected override void OnRun()
        {
            Task.Run(Listen);

            while (true)
            {
                Thread.Sleep(1000);

            }
        }
    }
}
