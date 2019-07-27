using System;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IPlayer : Orleans.IGrainWithIntegerKey
    {
        Task<bool> Login(string name);
    }
}
