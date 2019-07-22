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

        public ServiceManager(string dataPath = "../../../../Data/")
        {
            Config.DataPath = dataPath;
        }

        public void Start()
        {
            foreach(var service in services)
            {
                service.Init();
            }

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
