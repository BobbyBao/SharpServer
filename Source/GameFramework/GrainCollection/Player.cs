using GrainInterfaces;
using System;
using System.Threading.Tasks;

namespace GrainCollection
{
    public class Player : IPlayer
    {
        public Task<bool> Login(string name)
        {
            return Task.FromResult(true);
        }
    }
}
