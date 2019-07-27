using System;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IPlayerGrain : Orleans.IGrainWithIntegerKey
    {
        Task<bool> Login(string name);
    }
}
