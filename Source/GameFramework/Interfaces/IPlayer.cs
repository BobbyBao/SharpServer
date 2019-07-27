using System;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IPlayer : Orleans.IGrainWithIntegerKey
    {
        Task Login();
    }
}
