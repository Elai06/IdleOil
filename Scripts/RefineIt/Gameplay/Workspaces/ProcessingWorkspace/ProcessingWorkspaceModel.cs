using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Quests;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.Buildings.LevelBuildings;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.StaticData;
using Infrastructure.StaticData.ProcessingWorkspace;
using UnityEngine.AddressableAssets;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public class ProcessingWorkspaceModel : LevelBuildingBase<ProcessingWorkspaceProgress, ProcessingWorkspaceStaticData
        , ProcessingWorkspaceLevel>, IResourceCapacity, IExportStorage, IImportStorage
    {
        private readonly IStaticDataService _staticDataService;

        public readonly IStorageModel InputResourceStorage;
        public readonly IStorageModel OutputResourcesStorage;

        private readonly Dictionary<ResourceType, float> _bonusResources = new();

        public ProcessingWorkspaceModel(ProcessingWorkspaceProgress progress, ProcessingWorkspaceStaticData data,
            CurrenciesModel currenciesModel, IStaticDataService staticDataService, IQuestModel questModel) : base(
            progress,
            data, currenciesModel, questModel)
        {
            _staticDataService = staticDataService;
            InputResourceStorage = new StorageModel(Progress.RequiredStorage, this);
            OutputResourcesStorage = new StorageModel(Progress.ProduceStorage, this);
        }

        IStorageModel IExportStorage.ExportStorage => OutputResourcesStorage;
        IStorageModel IImportStorage.ImportStorage => InputResourceStorage;

        public event Action<ResourceType, int> CapacityChanged;
        public event Action<int> ProcessingTimer;
        public override string Id => Data.Type.ToString();
        public string Description => CurrentLevelData.Description;
        public ProcessingType ProcessingType => Data.Type;


        public int GetResourceCapacity(ResourceType resourceType)
        {
            if (TryGetResourceCapacity(resourceType, CurrentLevelData.RequiredStorageCapacity,
                    out var requiredResourceCapacity))
                return requiredResourceCapacity;
            if (TryGetResourceCapacity(resourceType, CurrentLevelData.ProduceStorageCapacity,
                    out requiredResourceCapacity))
                return requiredResourceCapacity;

            return 0;
            throw new InvalidOperationException($"Doesn't have Storage capacity for Resource {resourceType}");
        }

        public override void UpdateLevel()
        {
            base.UpdateLevel();
            UpdateCapacity(CurrentLevelData.RequiredStorageCapacity);
            UpdateCapacity(CurrentLevelData.ProduceStorageCapacity);
        }

        private void UpdateCapacity(ResourceCapacity[] requiredStorageCapacity)
        {
            foreach(var resourceCapacity in requiredStorageCapacity)
                CapacityChanged?.Invoke(resourceCapacity.ResourceType, resourceCapacity.Capacity);
        }

        private bool TryGetResourceCapacity(ResourceType resourceType, ResourceCapacity[] resourceCapacities,
            out int requiredResourceCapacity)
        {
            requiredResourceCapacity = 0;
            foreach (var requiredResource in resourceCapacities)
            {
                if (requiredResource.ResourceType != resourceType) continue;
                requiredResourceCapacity = requiredResource.Capacity;
                return true;
            }

            return false;
        }

        public AssetReference GetResourceSprite(ResourceType resourceResourceType)
        {
            return _staticDataService.GetResourceStaticData(resourceResourceType).SpriteAssetReference;
        }

        public void AddProcessingBonus(ResourceType resourceType, float value)
        {
            value /= 100;
            
            if (_bonusResources.ContainsKey(resourceType))
            {
                _bonusResources[resourceType] = value;
            }
            else
            {
                _bonusResources.Add(resourceType, value);
            }
        }

        public float GetResourceBonus(ResourceType resourceType)
        {
            if (_bonusResources.ContainsKey(resourceType))
            {
                 return _bonusResources[resourceType] + 1;
            }

            return 1;
        }

        public void ProcessingTime(int time)
        {
            ProcessingTimer?.Invoke(time);
        }
    }
}