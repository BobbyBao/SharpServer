using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Client
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;
    using SharpServer;

    public class PerfTestClientHandler : ChannelHandlerAdapter
    {
        public PerfTestClientHandler()
        {
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Task.Run(async () =>
            {
                IByteBuffer initialMessage = Unpooled.Buffer(1024);
                initialMessage.WriteBytes(Stats.testMsg);

                for (int i = 0; i < 100; i++)
                {
                    await context.WriteAndFlushAsync(initialMessage);
                    Interlocked.Increment(ref Stats.send);
                }
            });
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
}
