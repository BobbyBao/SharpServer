using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using GrainInterfaces;
using SharpServer;
using System.Threading.Tasks;

namespace GrainCollection
{
    public class World : Grain, IWorldGrain
    {
        public override Task OnActivateAsync()
        {
            Log.Info("Grain active : " + this.IdentityString);
            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            Log.Info("Grain deactive : " + this.IdentityString);
            return base.OnDeactivateAsync();
        }
    }
}
