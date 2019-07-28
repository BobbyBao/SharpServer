using System;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IPlayerGrain : Orleans.IGrainWithIntegerKey
    {
        Task<byte[]> SendMessage(int msgType, byte[] msg);
        Task<bool> Login(string name);
    }
}
