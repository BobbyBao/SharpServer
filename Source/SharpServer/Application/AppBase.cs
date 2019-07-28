using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace SharpServer
{
    public interface ITickable
    {
        void Tick(int msec);
    }

    public class AppBase
    {
        List<ISubsystem> services = new List<ISubsystem>();
        List<ITickable> tickables = new List<ITickable>();
        bool inited = false;
        protected ConcurrentDictionary<string, Connection> connections = new ConcurrentDictionary<string, Connection>();

        public int Interval
        {
            get; set;
        } = 1000;

        public AppBase()
        {
            Config.DataPath = "../../../../Data/";

            AddService<Log>();


        }

        public T AddService<T>() where T : ISubsystem, new()
        {
            T service = new T();
            services.Add(service);

            if(inited)
            {
                service.Init();
            }

            if(service is ITickable)
            {
                tickables.Add(service as ITickable);
            }

            return service;
        }

        public void Start()
        {
            foreach(var service in services)
            {
                service.Init();
            }

            inited = true;

            OnInit();

            OnStart();

            OnRun();

            OnShutdown();

            tickables.Clear();

            foreach (var service in services)
            {
                service.Shutdown();
            }

            services.Clear();
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnTick(int msec)
        {
            foreach(var tickable in tickables)
            {
                tickable.Tick(msec);
            }
        }

        protected virtual void OnRun()
        {
            Stopwatch sw = new Stopwatch();

            while (true)
            {
                sw.Reset();

                OnTick(Interval);

                int sleep = Interval - (int)sw.ElapsedMilliseconds;
                if(sleep > 0)
                {
                    Thread.Sleep(sleep);
                }
            }
        }

        protected virtual void OnShutdown()
        {
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
