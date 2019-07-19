using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public class AppBase
    {
        public virtual void Init()
        {
        }

        public virtual void Run()
        {
        }

        public virtual void Shutdown()
        {
        }
    }
}
