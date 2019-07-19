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

        public override void Start()
        {
            OnInit();
        }

        public async void Connect<T>() where T : IChannelHandler, new()
        {
            await NetworkClient.Connect<T>(IP, Port);
        }

        protected override void OnShutdown()
        {
        }
    }

    public class ClientApp<T> : ClientApp where T : IChannelHandler, new()
    {
        public override void Start()
        {
            OnInit();

            Connect<T>();
        }

    }
}
