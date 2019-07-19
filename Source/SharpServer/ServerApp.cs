﻿using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class ServerApp
    {
        public int Port { get; set; } 
        public NetworkServer Server { get; set; }
        public ServerApp(int port = 2239)
        {
            Port = port;
            Server = new NetworkServer();
        }

        public virtual void Run<T>() where T : IChannelHandler, new()
        {
            Server.Start<T>(Port).Wait();
        }

        public async Task Start<T>() where T : IChannelHandler, new()
        {
            await Server.Start<T>(Port);
        }
    }

    public class ServerApp<T> : ServerApp where T : IChannelHandler, new()
    {
        public void Start()
        {
            Run<T>();
        }

    }
}
