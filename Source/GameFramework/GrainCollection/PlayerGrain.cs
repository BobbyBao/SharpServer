using GrainInterfaces;
using System;
using System.Threading.Tasks;

namespace GrainCollection
{
    public class PlayerGrain : Orleans.Grain, IPlayerGrain
    {
        public Task<bool> Login(string name)
        {
            return Task.FromResult(true);
        }
    }
}
