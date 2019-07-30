
using System.Threading.Tasks;

namespace Test.Server
{
    class Program
    {
        static Task Main(string[] args)
        {
           return new PingPongServer().Start();
        }
    }
}
