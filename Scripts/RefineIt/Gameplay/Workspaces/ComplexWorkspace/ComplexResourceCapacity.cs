using System;
using Gameplay.Workspaces.MiningWorkspace;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    [Serializable]
    public struct ComplexResourceCapacity
    {
        public ResourceType ResourceType;
        public int Amount;
    }
}