//#define TEST_PINGPONG
#define TEST_PERF

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using SharpServer;

namespace Test.Server
{
    class Program
    {
        static void Main()
        {
#if TEST_PERF
            var svr = new RerfTestServer();
            svr.Start();
#else
            new SharpServer.ServerApp<EchoServer>().Start();
#endif
        }
    }
}
