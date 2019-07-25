using System;
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

    public class ServiceManager
    {
        List<IService> services = new List<IService>();
        List<ITickable> tickables = new List<ITickable>();
        bool inited = false;

        public int Interval
        {
            get; set;
        } = 1000;

        public ServiceManager()
        {
            Config.DataPath = "../../../../Data/";

            AddService<Log>();
        }

        public T AddService<T>() where T : IService, new()
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


    }
}
