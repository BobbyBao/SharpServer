using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class ClientApp : AppBase
    {
        public string IP { get; set; }
        public int Port { get; set; }

        protected override void OnInit()
        {
            NetworkClient.Init();            
        }

        public async void Connect<T>() where T : IChannelHandler, new()
        {
            await NetworkClient.Connect<T>(IP, Port);
        }

        protected override void OnShutdown()
        {
            NetworkClient.Shutdown();
        }
    }

    public class ClientApp<T> : ClientApp where T : IChannelHandler, new()
    {
        protected override void OnRun()
        {
            Connect<T>();
        }

    }
}
