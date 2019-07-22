using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
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

            var cfg = Config.Global;
            var cfgApp = Config.App;
        }

        protected override void OnInit()
        {
            NetworkServer.Init();
        }

        protected override void OnShutdown()
        {
            NetworkServer.Shutdown();

            base.OnShutdown();
        }

        public virtual void Listen<T>() where T : ServerHandler, new()
        {
            Server.Start<T>(Port, InitChannel).Wait();
        }

        protected virtual void InitChannel(IChannel channel)
        {
        }

    }

    public class ServerApp<T> : ServerApp where T : ServerHandler, new()
    {
        protected override void InitChannel(IChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;
            pipeline.AddLast(new LoggingHandler("SRV-CONN"));
            pipeline.AddLast("framing-enc", new LengthFieldPrepender(4));
            pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 4, 0, 4));

            T handler = new T();
            handler.server = Server;
            pipeline.AddLast("handler", handler);
        }

        public void DoListen()
        {
            Task.Run(() => Listen<T>());
        }


        protected override void OnRun()
        {
            Listen<T>();
        }

    }
}
