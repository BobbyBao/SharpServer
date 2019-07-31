using GrainInterfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainCollection
{
    public class Player : Grain, IPlayerGrain
    {
        public PlayerInfo info = new PlayerInfo();

        public override Task OnActivateAsync()
        {
            info.Id = this.GetPrimaryKeyLong();

            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {

            return base.OnDeactivateAsync();
        }
    }
}
