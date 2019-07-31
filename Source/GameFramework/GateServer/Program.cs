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
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await OrleansHelper.ConnectClient("dev", "gate"))
                {
                    var gateServer = new GateServer(client);
                    await gateServer.Start();

                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }


    }
}
