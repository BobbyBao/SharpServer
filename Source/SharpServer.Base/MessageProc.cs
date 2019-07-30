using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public interface IMessageProc
    {
        Task<byte> MsgProc(int msgType, byte[] body);
    }

    public class MessageProc : IMessageProc
    {
        public Task<byte> MsgProc(int msgType, byte[] body)
        {
            throw new NotImplementedException();
        }
    }
}
