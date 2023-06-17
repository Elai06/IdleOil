using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Currencies.Coefficients;
using Gameplay.Quests;
using Gameplay.Workspaces;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.StaticData;
using UnityEngine.AddressableAssets;

namespace Gameplay.Region.Storage
{
    public class RegionStorage : IRegionStorage
    {
        public event Action<ResourceType, int> ResourceChanged;
        public event Action<ResourceType> DictionaryResourceChanged;
        
        private IStorageModel _storageModel;

        private readonly CurrenciesModel _currenciesModel;
        private CurrencyCoefficientsStaticData _coefficientsStaticData;
        private readonly IWorkspaceService _workspaceService;
        private readonly IStaticDataService _staticDataService;
        private readonly IQuestModel _questModel;

        public RegionStorage(CurrenciesModel currenciesModel, IWorkspaceService workspaceService, IStaticDataService staticDataService, IQuestModel questModel)
        {
            _currenciesModel = currenciesModel;
            _workspaceService = workspaceService;
            _staticDataService = staticDataService;
            _questModel = questModel;
        }

        public IReadOnlyDictionary<ResourceType, ResourceProgress> GetDictionaryResources() => _storageModel.Resources;

        public void Initialize(StorageProgress storageProgress, IStaticDataService staticDataService)
        {
            _storageModel = new StorageModel(storageProgress, _workspaceService);
            _coefficientsStaticData = staticDataService.CurrencyCoefficientsStaticData;
        }

        event Action<ResourceType, int> IStorageModel.ResourceChanged
        {
            add => _storageModel.ResourceChanged += value;
            remove => _storageModel.ResourceChanged -= value;
        }
        
        event Action<ResourceType, int> IStorageModel.CapacityChanges
        {
            add => _storageModel.CapacityChanges += value;
            remove => _storageModel.CapacityChanges -= value;
        }

        public IReadOnlyDictionary<ResourceType, ResourceProgress> Resources => _storageModel.Resources;

        public void AddResources(ResourceType resourceType, int amount)
        {
            var oldResourceCount =  _storageModel.Resources.Count;

            var canPlaceResources = _storageModel.CanPlaceResources(resourceType);
            if (amount > canPlaceResources)
            {
                var resourcesForSell = amount - canPlaceResources;
               _storageModel.AddResources(resourceType, canPlaceResources);
                SellResource(resourceType, resourcesForSell);
            }
            else
            {
                _storageModel.AddResources(resourceType, amount);
            }
            
            if (_storageModel.Resources.Count > oldResourceCount)
            {
                DictionaryResourceChanged?.Invoke(resourceType);
                
                return;
            }
            
            ResourceChanged?.Invoke(resourceType, amount);
        }

        public int CanPlaceResources(ResourceType resourceType)
        {
            return int.MaxValue;
        }

        public bool CanTakeResources(ResourceType resourceType, int spendingResources)
        {
            return _storageModel.CanTakeResources(resourceType, spendingResources);
        }

        public int CanTakeResources(ResourceType resourceType)
        {
            return _storageModel.CanTakeResources(resourceType);
        }

        public void TakeResources(ResourceType resourceType, int spendingResources)
        {
            _storageModel.TakeResources(resourceType, spendingResources);
        }

        public int GetResourceCapacity(ResourceType resourceType)
        {
            return _workspaceService.GetComplexResourceCapacity(resourceType) +
                   _workspaceService.GetStorageOilCrudeCapacity(resourceType);
        }

        public AssetReference GetResourceSprite(ResourceType type)
        {
            return _staticDataService.GetResourceStaticData(type).SpriteAssetReference;
        }

        private void SellResource(ResourceType resourceType, int amount)
        {
            _currenciesModel.Add(CurrencyType.SoftCurrency, СonvertResourceCurrency(resourceType, amount));
            _questModel.TaskDailyProgress(QuestsGuid.sellResource, amount);
            _questModel.TaskWeeklyProgress(QuestsGuid.sellResourceWeek, amount);
        }

        private int СonvertResourceCurrency(ResourceType resourceType, int amount)
        {
            foreach (var value in _coefficientsStaticData.CurrencyCoefficientsData)
            {
                if (value.ResourceType == resourceType)
                    return value.PriceForOne * amount;
            }

            throw new InvalidOperationException($"Conversion config doesn't have Resource with type {resourceType}");
        }
    }
}