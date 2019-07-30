using SharpServer;
using System;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IGateGrain : Orleans.IGrainWithIntegerKey
    {
        Task<byte[]> SendMessage(int msgType, byte[] msg);
    }
}
