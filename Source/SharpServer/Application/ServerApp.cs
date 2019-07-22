using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class ServerApp : ServiceManager
    {
        public int Port { get; set; } 
        public NetworkServer Server { get; set; }
        public ServerApp(int port = 2239)
        {
            Port = port;
            Server = new NetworkServer();
        }

        protected override void OnShutdown()
        {
            Server?.Shutdown();

            base.OnShutdown();
        }

        public virtual void Listen<T>() where T : ServerHandler, new()
        {
            Server.Start<T>(Port).Wait();
        }

    }

    public class ServerApp<T> : ServerApp where T : ServerHandler, new()
    {
        public void DoListen()
        {
            Task.Run(() => Listen<T>());
        }

        protected override void OnRun()
        {
            Server.Start<T>(Port).Wait();
        }

    }
}
