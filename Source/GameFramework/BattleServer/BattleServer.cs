using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleServer
{
    public class BattleServer : ServerApp
    {
        public BattleServer(string[] args) : base(args)
        {
        }

        protected override void OnConnect(Connection handler)
        {
            //handler.Register<UserLoginReqT>((int)MessageType.UserLoginReq, HandleUserLoginReq);
        }

        /*
        void HandleUserLoginReq(UserLoginReqT msg)
        {
            Log.Info("User {0}, login", msg.UserName);

            var res = new UserLoginResT
            {
                Res = 0
            };

            handler.Send((int)MessageType.UserLoginRes, res);
        }*/

    }
}
