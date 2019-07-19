using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class AppBase
    {
        public virtual void Start()
        {
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnIdle()
        {
        }

        protected virtual void OnShutdown()
        {
        }


    }
}
