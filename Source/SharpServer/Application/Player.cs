using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class Player : IDisposable
    {
        public long id;
        public Connection conn;

        public Player()
        {
        }

        public virtual void Connect(Connection conn)
        {
            this.conn = conn;
        }

        public virtual void Disconnect()
        {
            this.conn = null;
        }

        public virtual void Dispose()
        {
        }

        public virtual void Tick(int msec)
        {

        }
    }
}
