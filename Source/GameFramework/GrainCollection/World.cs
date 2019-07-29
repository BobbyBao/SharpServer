using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using GrainInterfaces;

namespace GrainCollection
{
    public class World : Grain, IWorldGrain
    {
    }
}
