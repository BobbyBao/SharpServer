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
        bool inited = false;

        public int Interval { get; set; } = 1000;

        public AppBase()
        {
            Config.DataPath = "../../../../Data/";

            AddService<Log>();
        }

        public virtual T AddService<T>() where T : new()
        {
            T service = new T();
            services.Add(service);

            if(inited)
            {
                (service as ISubsystem)?.Init();
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

            foreach (var service in services)
            {
                (service as ISubsystem)?.Shutdown();
            }

            services.Clear();
        }

        protected virtual async Task OnInit()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task OnStart()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task OnRun()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task OnShutdown()
        {
            await Task.CompletedTask;
        }

    }
}
