using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class PlayerManager : AbstractService, ITickable
    {
        ConcurrentDictionary<long, Player> players = new ConcurrentDictionary<long, Player>();

        public PlayerManager()
        {
        }

        public T CreatePlayer<T>(long id, Connection conn = null) where T : Player, new()
        {
            var player = new T
            {
                id = id,
                conn = conn
            };

            players.TryAdd(id, player);
            return player;
        }

        public T GetPlayer<T>(long id, Connection conn = null) where T : Player, new()
        {
            if(players.TryGetValue(id, out var player))
            {
                return player as T;
            }

            return CreatePlayer<T>(id, conn);
        }

        public void DestroyPlayer(long id)
        {
            if (players.TryRemove(id, out var player))
            {
                player.Dispose();
            }

        }

        public override void Shutdown()
        {
            var it = players.GetEnumerator();
            while(it.MoveNext())
            {
                it.Current.Value.Dispose();
            }

            players.Clear();
        }

        public void Tick(int msec)
        {
        }
    }
}
