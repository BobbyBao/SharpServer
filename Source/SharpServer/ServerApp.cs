using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class ServerApp
    {
        public NetworkServer Server { get; set; }
        public ServerApp()
        {
            Server = new NetworkServer();
        }

        public async Task Run<T>(int port) where T : IChannelHandler, new()
        {
            await Server.Start<T>(port);
        }
    }
}
