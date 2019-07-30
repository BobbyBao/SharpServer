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
    public class ServerApp : NetworkApp
    {
        public int Port { get; set; } = 2239;
        public NetworkServer Server { get; set; }
        public ServerApp()
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

        protected override Task OnInit()
        {
            NetworkServer.Init();
            return Task.CompletedTask;
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

        protected override Task OnStart()
        {
            return Listen();
        }

        public async virtual Task Listen()
        {
            await Server.Start(Port, InitChannel);
        }

        protected override async Task OnShutdown()
        {
            await Server.Close();

            await NetworkServer.Shutdown();

            await base.OnShutdown();
        }

    }

}
