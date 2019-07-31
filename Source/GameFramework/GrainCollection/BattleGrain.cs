using GrainInterfaces;
using SharpServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainCollection
{
    public class BattleGrain : Orleans.Grain, IGateBattle
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

        public Task<byte[]> SendMessage(int msgType, byte[] msg)
        {
            return null;
        }
    }
}
