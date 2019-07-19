namespace Test.Server
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels;

    /// <summary>
    /// 服务端处理事件函数
    /// </summary>
    public class EchoServer : SharpServer.NetworkServer.MessageHandler
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            context.WriteAsync(message);
        }

    }
}
