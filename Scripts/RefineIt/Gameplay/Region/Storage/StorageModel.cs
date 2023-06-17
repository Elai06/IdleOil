using System;
using System.Collections.Generic;
using Gameplay.Workspaces.MiningWorkspace;

namespace Gameplay.Region.Storage
{
    public class StorageModel : IStorageModel
    {
        public event Action<ResourceType, int> ResourceChanged;

        private readonly Dictionary<ResourceType, ResourceProgress> _resources = new();

        private readonly StorageProgress _storageProgress;
        private readonly IResourceCapacity _resourceCapacity;

        public StorageModel(StorageProgress storageProgress, IResourceCapacity resourceCapacity)
        {
            _storageProgress = storageProgress;
            _resourceCapacity = resourceCapacity;
            foreach (var value in storageProgress.ResourceProgress)
                _resources[value.ResourceType] = value;
        }
        
        event Action<ResourceType, int> IStorageModel.CapacityChanges
        {
            add => _resourceCapacity.CapacityChanged += value;
            remove => _resourceCapacity.CapacityChanged -= value;
        }

        public IReadOnlyDictionary<ResourceType, ResourceProgress> Resources => _resources;

        public void AddResources(ResourceType resourceType, int addedResources)
        {
            var resourceProgress = GetResourceProgress(resourceType);
            if (addedResources + resourceProgress.Amount > _resourceCapacity.GetResourceCapacity(resourceType))
                throw new InvalidOperationException($"Storage crowded");

            resourceProgress.Amount += addedResources;

            ResourceChanged?.Invoke(resourceProgress.ResourceType, resourceProgress.Amount);
        }

        public int CanPlaceResources(ResourceType resourceType) =>
            _resourceCapacity.GetResourceCapacity(resourceType) - GetResourceProgress(resourceType).Amount;

        public bool CanTakeResources(ResourceType resourceType, int spendingResources) =>
            GetResourceProgress(resourceType).Amount >= spendingResources;

        public int CanTakeResources(ResourceType resourceType) => 
            GetResourceProgress(resourceType).Amount;

        private ResourceProgress GetResourceProgress(ResourceType resourceType)
        {
            if (_resources.ContainsKey(resourceType))
                return _resources[resourceType];

            var updatedResource = _storageProgress.GetOrCreate(resourceType);
            _resources.Add(resourceType, updatedResource);

            return updatedResource;
        }

        public void TakeResources(ResourceType resourceType, int spendingResources)
        {
            var resourceProgress = GetResourceProgress(resourceType);
            if (resourceProgress.Amount >= spendingResources)
                resourceProgress.Amount -= spendingResources;

            ResourceChanged?.Invoke(resourceProgress.ResourceType, resourceProgress.Amount);
        }

        public int GetResourceCapacity(ResourceType resourceType)
        {
           return _resourceCapacity.GetResourceCapacity(resourceType);
        }
    }
}