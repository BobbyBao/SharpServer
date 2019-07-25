using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
            {
                    Port = appCfg.GetValue("Port", 2239);
                var log = appCfg.GetSection("Log");
            }

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

            Connection handler = CreateConnection();
            handler.connected += OnConnect;
            handler.disconnected += OnDisconnect;
            pipeline.AddLast("handler", handler);
        }

        protected override void OnStart()
        {
            Task.Run(Listen);
        }

        public async virtual Task Listen()
        {
            await Server.Start(Port, InitChannel);
        }

        protected virtual Connection CreateConnection()
        {
            return new Connection();
        }

        protected virtual void OnConnect(Connection conn)
        {
        }

        protected virtual void OnDisconnect(Connection conn)
        {
        }

        protected override void OnShutdown()
        {
            NetworkServer.Shutdown();

            base.OnShutdown();
        }

    }

}
