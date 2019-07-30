using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class ClientApp : NetworkApp
    {
        public string IP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 2239;

        protected override Task OnInit()
        {
            NetworkClient.Init();
            return Task.CompletedTask;
        }

        protected virtual void InitChannel(ISocketChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;
            pipeline.AddLast(new LoggingHandler());
            pipeline.AddLast("framing-enc", new MsgEncoder());
            pipeline.AddLast("framing-dec", new MsgDecoder());

            var handler = CreateConnection();
            handler.connected += OnConnect;
            handler.disconnected += OnDisconnect;

            pipeline.AddLast("handler", handler);
        }

        public async Task Connect()
        {
            await NetworkClient.Connect(IP, Port, InitChannel);
        }
        
        protected override Task OnShutdown()
        {
            NetworkClient.Shutdown();
            return Task.CompletedTask;
        }
    }

}
