using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class AppBase
    {

        public void Start()
        {
            OnInit();

            OnRun();

            OnShutdown();
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
