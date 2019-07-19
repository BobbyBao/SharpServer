//#define TEST_PINGPONG
#define TEST_PERF


namespace Test.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using DotNetty.Buffers;
    using DotNetty.Codecs;
    using DotNetty.Handlers.Logging;
    using DotNetty.Transport.Bootstrapping;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Sockets;
    using SharpServer;

    static class Program
    {
#if TEST_PERF
        static void Main() => new PerfTestClient().Start();
#else
        
        static void Main() => new ClientApp<EchoClientHandler>().Start();
#endif
    }
}
