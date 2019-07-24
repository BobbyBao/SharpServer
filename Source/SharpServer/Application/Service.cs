using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public interface IService
    {
        void Init();
        void Shutdown();
    }

    public abstract class AbstractService : IService
    {
        public virtual void Init()
        {
        }

        public virtual void Shutdown()
        {
        }
    }
}
