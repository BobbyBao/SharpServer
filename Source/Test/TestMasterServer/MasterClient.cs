using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestMasterServer
{
    public class MasterClientHandler : ChannelHandlerAdapter
    {
        public MasterClientHandler()
        {
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            Interlocked.Increment(ref Stats.recv);
            context.WriteAsync(message);
            Interlocked.Increment(ref Stats.send);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }

    public class MasterClient : ClientApp<MasterClientHandler>
    {
        public MasterClient()
        {
        }

        protected override void OnRun()
        {
            for (int i = 0; i < 3000; i++)
            {
                Task.Run(async () =>
                {
                    IChannel channel = await Connect<MasterClientHandler>();

                    for (int j = 0; j < 200; j++)
                    {
                        Task.Run(async () =>
                        {
                            IByteBuffer initialMessage = Unpooled.Buffer(128);
                            initialMessage.WriteBytes(Stats.testMsg);

                            await channel.WriteAndFlushAsync(initialMessage);
                        });
                    }

                });
            }

            int lastRecv = 0;
            int lastSend = 0;

            while (true)
            {
                Thread.Sleep(1000);

                Console.WriteLine("Send {0}, Receive {1} per sec", (int)(Stats.send - lastSend), (int)(Stats.recv - lastRecv));
                lastRecv = Stats.recv;
                lastSend = Stats.send;
            }

        }
    }
}
