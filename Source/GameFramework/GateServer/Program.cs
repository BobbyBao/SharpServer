using Orleans.Configuration;
using Orleans;
using System;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.Extensions.Logging;
using SharpServer;

namespace GateServer
{
    class Program
    {
        static Task Main(string[] args) => new GateServer().Start();
    }
}
