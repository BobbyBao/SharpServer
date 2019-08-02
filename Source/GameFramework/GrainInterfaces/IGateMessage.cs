using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public delegate Task<byte[]> MessageHandler(byte[] msg);

    public interface IGateMessage
    {
        Task<byte[]> SendMessage(int msgType, byte[] msg);
    }

    public interface IGateMaster : Orleans.IGrainWithIntegerKey, IGateMessage
    {
    }

    public interface IGateBattle : Orleans.IGrainWithIntegerKey, IGateMessage
    {
    }
}
