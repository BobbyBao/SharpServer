using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class ClientApp : ServiceManager
    {
        public string IP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 2239;

        protected override void OnInit()
        {
            NetworkClient.Init();            
        }

        public async Task<IChannel> Connect<T>() where T : IChannelHandler, new()
        {
            return await NetworkClient.Connect<T>(IP, Port);
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
            Connect<T>().Wait();
        }

    }
}
