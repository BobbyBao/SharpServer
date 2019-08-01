using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using MasterServer;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    public class TestClient : ClientApp
    {
        public TestClient()
        {
        }

        protected override void OnConnect(Connection handler)
        {
            handler.Register<UserLoginRes>((int)MessageType.UserLoginRes, HandleUserLoginRes);

            var req = new UserLoginReq
            {
                UserName = "Test"
            };

            handler.Send((int)MessageType.UserLoginReq, req);
        }

        void HandleUserLoginRes(UserLoginRes msg)
        {
            if(msg.Res == 0)
            {
                Log.Info("User {0}, login succ.", new Guid(msg.UserId));
            }
            else
            {
                Log.Info("User {0}, login failed!", new Guid(msg.UserId));
            }
        }

        protected override Task OnStart()
        {
            //for (int i = 0; i < 3000; i++)
            {
                return Connect();
            }

        }

    }
}
