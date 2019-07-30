using System.Threading.Tasks;

namespace Test.Client
{
    static class Program
    {
        static Task Main() => new PingPongClient().Start();
    }
}
