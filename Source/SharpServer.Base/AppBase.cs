using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer
{
    public interface ITickable
    {
        void Tick(int msec);
    }

    public class AppBase
    {
        List<object> services = new List<object>();
        List<ITickable> tickables = new List<ITickable>();
        bool inited = false;

        public int Interval
        {
            get; set;
        } = 1000;

        public AppBase()
        {
            Config.DataPath = "../../../../Data/";

            AddService<Log>();


        }

        public T AddService<T>() where T : new()
        {
            T service = new T();
            services.Add(service);

            if(inited)
            {
                (service as ISubsystem)?.Init();
            }

            if(service is ITickable)
            {
                tickables.Add(service as ITickable);
            }

            return service;
        }

        public async Task Start()
        {
            foreach(var service in services)
            {
                (service as ISubsystem)?.Init();
            }

            inited = true;

            await OnInit();

            await OnStart();

            await OnRun();

            await OnShutdown();

            tickables.Clear();

            foreach (var service in services)
            {
                (service as ISubsystem)?.Shutdown();
            }

            services.Clear();
        }

        protected virtual Task OnInit()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnStart()
        {
            return Task.CompletedTask;
        }

        protected virtual void OnTick(int msec)
        {
            foreach(var tickable in tickables)
            {
                tickable.Tick(msec);
            }
        }

        protected virtual Task OnRun()
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

            return Task.CompletedTask;
        }

        protected virtual Task OnShutdown()
        {
            return Task.CompletedTask;
        }

    }
}
