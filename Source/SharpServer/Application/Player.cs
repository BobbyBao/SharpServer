using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class Player : IDisposable
    {
        public long id;
        public MsgHandler conn;

        public Player()
        {
        }

        public void Connect(MsgHandler conn)
        {
            this.conn = conn;
        }

        public void Disconnect()
        {
            this.conn = null;
        }

        public void Dispose()
        {

        }
    }
}
