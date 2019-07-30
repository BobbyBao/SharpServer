using System;
using System.Collections.Generic;
using System.Text;

namespace SharpServer
{
    public interface ISubsystem
    {
        void Init();
        void Shutdown();
    }

    public abstract class Subsystem : ISubsystem
    {
        public virtual void Init()
        {
        }

        public virtual void Shutdown()
        {
        }
    }
}
