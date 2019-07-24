using Benchmark;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using ServiceStack;
using System;
using System.Diagnostics;
using System.IO;

#if NETCOREAPP3_0
using System.Text.Json.Serialization;
#endif
using System.Threading;

namespace Benchmark
{
    
    class Program
    {
        static void Main(string[] args)
        {
            //var test = new TestJson();

            BenchmarkRunner.Run<TestJson>();
        }
    }

}
