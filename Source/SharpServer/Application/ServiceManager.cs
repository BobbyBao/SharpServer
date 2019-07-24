using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpServer
{

    public interface IService
    {
        void Init();
        void Shutdown();
    }

    public class ServiceManager
    {
        List<IService> services = new List<IService>();
        bool inited = false;
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

            OnRun();

            OnShutdown();

            foreach (var service in services)
            {
                service.Shutdown();
            }
        }


        protected virtual void OnInit()
        {
        }

        protected virtual void OnIdle()
        {
        }

        protected virtual void OnRun()
        {
        }

        protected virtual void OnShutdown()
        {
        }


    }
}
