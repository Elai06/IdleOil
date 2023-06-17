using System;
using Gameplay.Workspaces.MiningWorkspace;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    [Serializable]
    public class ResourceCapacity
    {
        public ResourceType ResourceType;
        public int Capacity;
    }
}