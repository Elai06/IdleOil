using System;
using Gameplay.Workspaces.MiningWorkspace;

namespace Gameplay.Region.Storage
{
    public interface IResourceCapacity
    {
        event Action<ResourceType, int> CapacityChanged;
        int GetResourceCapacity(ResourceType resourceType);
    }
}