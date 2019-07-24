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
        public int Port { get; set; } = 2239;
        public NetworkServer Server { get; set; }
        public ServerApp(string[] args)
        {
            Server = new NetworkServer();

            var cfg = Config.Global;
            var appCfg = Config.App;
            if(appCfg != null)
            Port = appCfg.GetValue("port", 2239);

        }

        protected override void OnInit()
        {
            NetworkServer.Init();
        }

        protected virtual void InitChannel(IChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;
            pipeline.AddLast(new LoggingHandler("SRV-CONN"));
            pipeline.AddLast("framing-enc", new MsgEncoder());
            pipeline.AddLast("framing-dec", new MsgDecoder());

            MsgHandler handler = CreateHandler();
            handler.channelRegistered += OnConnect;
            handler.channelUnregistered += OnDisconnect;
            pipeline.AddLast("handler", handler);
        }

        protected override void OnRun()
        {
            Listen().Wait();
        }

        public async virtual Task Listen()
        {
            await Server.Start(Port, InitChannel);
        }

        protected virtual MsgHandler CreateHandler()
        {
            return new MsgHandler();
        }

        protected virtual void OnConnect(MsgHandler context)
        {
        }

        protected virtual void OnDisconnect(MsgHandler context)
        {
        }

        protected override void OnShutdown()
        {
            NetworkServer.Shutdown();

            base.OnShutdown();
        }

    }

    public class ServerApp<T> : ServerApp where T : MsgHandler, new()
    {
        public ServerApp(string[] args) : base(args)
        {
        }

        protected override MsgHandler CreateHandler()
        {
            return new T();
        }


    }
}
