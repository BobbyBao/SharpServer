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
    public class ClientApp : ServiceManager
    {
        public string IP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 2239;

        protected override void OnInit()
        {
            NetworkClient.Init();            
        }

        protected virtual void InitChannel(ISocketChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;
            pipeline.AddLast(new LoggingHandler());
            pipeline.AddLast("framing-enc", new MsgEncoder());
            pipeline.AddLast("framing-dec", new MsgDecoder());

            var handler = CreateHandler();
            handler.channelRegistered += OnConnect;
            handler.channelUnregistered += OnDisconnect;

            pipeline.AddLast("handler", handler);
        }

        protected virtual MsgHandler CreateHandler()
        {
            return new MsgHandler();
        }

        public async Task Connect()
        {
            await NetworkClient.Connect(IP, Port, InitChannel);
        }

        protected virtual void OnConnect(MsgHandler context)
        {
        }

        protected virtual void OnDisconnect(MsgHandler context)
        {
        }

        protected override void OnRun()
        {
            Connect().Wait();
        }

        protected override void OnShutdown()
        {
            NetworkClient.Shutdown();
        }
    }

    public class ClientApp<T> : ClientApp where T : MsgHandler, new()
    {
        protected override MsgHandler CreateHandler()
        {
            return new T();
        }


    }
}
