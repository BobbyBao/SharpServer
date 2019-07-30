using System;
using System.Threading.Tasks;

namespace SharpServer
{
    public class SiloApp : AppBase
    {
        protected override Task OnInit()
        {
            return Task.CompletedTask;
        }

        protected override Task OnRun()
        {
            return Task.CompletedTask;
        }
    }
}
