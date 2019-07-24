using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class PlayerManager : AbstractService
    {
        ConcurrentDictionary<long, Player> players = new ConcurrentDictionary<long, Player>();

        public PlayerManager()
        {
        }


        public Player CreatePlayer(long id, MsgHandler conn = null)
        {
            var player = new Player
            {
                id = id,
                conn = conn
            };

            players.TryAdd(id, player);
            return player;
        }

        public Player GetPlayer(long id, MsgHandler conn = null)
        {
            if(players.TryGetValue(id, out var player))
            {
                return player;
            }

            return CreatePlayer(id, conn);
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
        }
    }
}
