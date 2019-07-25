using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class AppBase : ServiceManager
    {
        protected ConcurrentDictionary<string, Connection> connections = new ConcurrentDictionary<string, Connection>();
        protected PlayerManager playerMgr;
        public AppBase()
        {
            playerMgr = AddService<PlayerManager>();
        }

        protected virtual Connection CreateConnection()
        {
            return new Connection();
        }

        protected virtual void OnConnect(Connection conn)
        {
            connections.TryAdd(conn.channelID, conn);
        }

        protected virtual void OnDisconnect(Connection conn)
        {
            connections.TryRemove(conn.channelID, out var c);
        }

    }
}
