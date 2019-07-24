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

        public async Task Connect<T>() where T : IChannelHandler, new()
        {
            await NetworkClient.Connect<T>(IP, Port, InitChannel);
        }

        protected virtual void InitChannel(ISocketChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;
            pipeline.AddLast(new LoggingHandler());
            pipeline.AddLast("framing-enc", new MsgEncoder());
            pipeline.AddLast("framing-dec", new MsgDecoder());
            pipeline.AddLast("echo", new MsgHandler());
        }

        protected override void OnShutdown()
        {
            NetworkClient.Shutdown();
        }
    }

    public class ClientApp<T> : ClientApp where T : IChannelHandler, new()
    {
        protected override void InitChannel(ISocketChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;
            pipeline.AddLast(new LoggingHandler());
            pipeline.AddLast("framing-enc", new MsgEncoder());
            pipeline.AddLast("framing-dec", new MsgDecoder());
            pipeline.AddLast("echo", new T());
        }

        public void DoConnect()
        {
            Task.Run(async () =>
            {
                await Connect<T>();
            });
        }

        protected override void OnRun()
        {
            Connect<T>().Wait();
        }

    }
}
