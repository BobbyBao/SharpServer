using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer
{
    public class NetworkApp : AppBase
    {
        List<ITickable> tickables = new List<ITickable>();
        protected ConcurrentDictionary<string, Connection> connections = new ConcurrentDictionary<string, Connection>();

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

        public override T AddService<T>()
        {
            var service = base.AddService<T>();

            if (service is ITickable)
            {
                tickables.Add(service as ITickable);
            }
            return service;
        }

        protected virtual void OnTick(int msec)
        {
            foreach (var tickable in tickables)
            {
                tickable.Tick(msec);
            }
        }

        protected override async Task OnRun()
        {
            if(Interval < 0)
            {
                Console.ReadKey();
                return;
            }

            Stopwatch sw = new Stopwatch();

            while (true)
            {
                sw.Reset();

                OnTick(Interval);

                int sleep = Interval - (int)sw.ElapsedMilliseconds;
                if (sleep > 0)
                {
                    Thread.Sleep(sleep);
                }
            }

            await Task.CompletedTask;
        }

        protected override async Task OnShutdown()
        {

            tickables.Clear();

            await base.OnShutdown();
        }
    }
}
