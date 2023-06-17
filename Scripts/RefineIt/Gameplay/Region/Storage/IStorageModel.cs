using System;
using System.Collections.Generic;
using Gameplay.Workspaces.MiningWorkspace;

namespace Gameplay.Region.Storage
{
    public interface IStorageModel
    {
        event Action<ResourceType, int> ResourceChanged;
        event Action<ResourceType, int> CapacityChanges;
        IReadOnlyDictionary<ResourceType, ResourceProgress> Resources { get; }
        void AddResources(ResourceType resourceType, int addedResources);
        int CanPlaceResources(ResourceType resourceType);
        bool CanTakeResources(ResourceType resourceType, int spendingResources);
        public int CanTakeResources(ResourceType resourceType);
        void TakeResources(ResourceType resourceType, int spendingResources);
        int GetResourceCapacity(ResourceType resourceType);
    }
}