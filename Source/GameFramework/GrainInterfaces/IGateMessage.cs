using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
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
