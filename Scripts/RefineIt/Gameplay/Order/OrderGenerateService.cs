using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay.Currencies;
using Gameplay.Orders;
using Gameplay.Quests;
using Gameplay.Region.Storage;
using Gameplay.Services.Timer;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.AssetManagement;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.StaticData.ProcessingWorkspace;
using Infrastructure.YandexAds;
using UnityEngine;
using Utils.Extensions;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Order
{
    public class OrderGenerateService
    {
        public event Action<OrderModel> UpdateOrder;
        public event Action<OrderModel> CreatedOrder;
        public event Action<OrderModel> RemoveOrderInDictionary;

        public readonly Dictionary<int, OrderModel> OrderModels = new();

        private IStaticDataService _staticDataService;
        private IProgressService _progressService;
        private IAssetProvider _assetProvider;
        private IRegionStorage _regionStorage;
        private CurrenciesModel _currenciesModel;
        private TimerService _timerService;
        private OrderAdsStatus _orderAdsStatus;
        private IAdsService _yandexAdsService;
        private IQuestModel _questModel;

        private Dictionary<ResourceType, float> _resourceOutput = new();

        [Inject]
        public void Construct(IStaticDataService staticDataService, IProgressService progressService,
            IAssetProvider assetProvider, IRegionStorage regionStorage, CurrenciesModel currenciesModel,
            TimerService timerService, IAdsService yandexAdsService, IQuestModel questModel)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            _assetProvider = assetProvider;
            _regionStorage = regionStorage;
            _currenciesModel = currenciesModel;
            _timerService = timerService;
            _yandexAdsService = yandexAdsService;
            _questModel = questModel;

            _orderAdsStatus = new OrderAdsStatus(progressService, timerService);
        }

        private OrderGenerateConfigData OrderGenerateConfigData => _staticDataService.OrderConfigData.Data;

        public OrderAdsStatus AdsStatus => _orderAdsStatus;

        public void Initialize()
        {
            if (_progressService.RegionProgress.IsTutorial)
            {
                return;
            }

            CreateOrderModels();
            _orderAdsStatus.Initialize();
        }

        private void CreateOrderModels()
        {
            for (int i = 0; i < OrderGenerateConfigData.OrderCount; i++)
            {
                TimeProgress timerProgress = _progressService.RegionProgress.TimeOrderProgresses.TimeProgresses
                    .Find(x => x.ID == $"{TimerType.CreatedOrder}{i}");

                int delay = timerProgress?.Time ?? 0;

                if (delay > 0)
                {
                    if (timerProgress != null)
                    {
                        TimeModel timeModel =
                            _timerService.CreateTimer($"{TimerType.CreatedOrder}{i}", timerProgress.Time);
                        timeModel.Stopped += CreateOrderLater;
                        continue;
                    }
                }

                CreateOrder(i);
            }
        }

        private async void CreateOrder(int index)
        {
            List<OrderResourceData> resourcesData = await CreateResourcesData();
            OrderRewardData orderRewardData = await CreateOrderRewardData(resourcesData);

            OrderProgress orderProgress = _progressService.RegionProgress.OrdersProgress
                .GetOrCreate(index, orderRewardData, resourcesData, OrderGenerateConfigData.TimeToFailed,
                    _orderAdsStatus.IsCanShowAds);

            await CheckSpriteOnNull(orderProgress);

            OrderModel orderModel = new(_regionStorage, _currenciesModel, _timerService, orderProgress,
                _yandexAdsService, _questModel);

            OrderModels.Add(index, orderModel);

            OrderModels[orderProgress.ID].ClientSprite = await GetClientSprite(orderProgress);
            
            CreatedOrder?.Invoke(orderModel);
            orderModel.UpdateOrderStatus += OnUpdateOrderStatus;
        }

        public async Task<Sprite> GetClientSprite(OrderProgress progress)
        {
            var index = progress.ClientSpriteIndex == -1
                ? Random.Range(0, _staticDataService.ClientSpriteStorage.ClientSprites.Count)
                : progress.ClientSpriteIndex;

            if (index == -1)
            {
                for (int i = 0; i < OrderModels.Count; i++)
                {
                    var model = OrderModels.Values.ToList()[i];

                    while (model.OrderProgress.ClientSpriteIndex == index)
                    {
                        index = Random.Range(0, _staticDataService.ClientSpriteStorage.ClientSprites.Count);
                    }
                }
            }

            progress.ClientSpriteIndex = index;

            return await _assetProvider.LoadSprite(_staticDataService.ClientSpriteStorage.ClientSprites[index]);
        }

        private void CreateOrderLater(TimeModel timeModel)
        {
            string id = timeModel.TimeProgress.ID.Remove(0, timeModel.TimeProgress.ID.Length - 1);

            CreateOrder(int.Parse(id));

            timeModel.Stopped -= CreateOrderLater;
        }

        private void CreateNewOrderLater(OrderModel orderModel)
        {
            RemoveOrder(orderModel);

            int delay = Random.Range(FormatTime.MinutesIntFormat(OrderGenerateConfigData.MinDelayNextOrder),
                FormatTime.MinutesIntFormat(OrderGenerateConfigData.MaxDelayNextOrder));

            string timeId = $"{TimerType.CreatedOrder}{orderModel.ID}";

            TimeModel timeModel = _timerService.CreateTimer(timeId, delay);

            timeModel.Stopped += CreateOrderLater;
        }

        private void OnUpdateOrderStatus(OrderModel orderModel)
        {
            if (orderModel.OrderProgress.OrderStatus == OrderStatus.Complete ||
                orderModel.OrderProgress.OrderStatus == OrderStatus.Failed)
            {
                CreateNewOrderLater(orderModel);

                return;
            }

            if (orderModel.OrderProgress.OrderStatus == OrderStatus.AdsReplace)
            {
                /*_timerService.GetTimer(TimerType.ToAds.ToString(),
                    FormatTime.HoursIntFormat(_staticDataService.OrderConfigData.Data.TimeToAds));*/

                ReplaceOrder(orderModel);
                return;
            }

            UpdateOrder?.Invoke(OrderModels[orderModel.ID]);
        }

        private void ReplaceOrder(OrderModel orderModel)
        {
            _timerService.CreateTimer(TimerType.ToAds.ToString(), 30);

            RemoveOrder(orderModel);
            CreateOrder(orderModel.ID);

            _orderAdsStatus.UpdateOrderAdsStatus();
        }

        private void RemoveOrder(OrderModel orderModel)
        {
            _progressService.RegionProgress.OrdersProgress.OrderProgresses.Remove(orderModel.OrderProgress);
            OrderModels.Remove(orderModel.ID);
            RemoveOrderInDictionary?.Invoke(orderModel);
        }

        private async Task<List<OrderResourceData>> CreateResourcesData()
        {
            FillResourcesQuantity();

            int maxSlot = _resourceOutput.Count >= OrderGenerateConfigData.MaxSlotQuantity
                ? OrderGenerateConfigData.MaxSlotQuantity
                : _resourceOutput.Count;
            int slotQuantity = GenerateIntRandom(OrderGenerateConfigData.MinSlotQuantity, maxSlot);

            List<OrderResourceData> resourcesData = new();

            for (int i = 0; i < slotQuantity; i++)
            {
                ResourceType randomResourceType = GetRandomResourceType();

                OrderResourceData resourceData = new()
                {
                    ResourceType = randomResourceType,
                    Quantity = GetResourceQuantity(randomResourceType),
                    ResourceSprite = await _assetProvider.LoadSprite(_staticDataService
                        .GetResourceStaticData(randomResourceType).SpriteAssetReference)
                };

                resourcesData.Add(resourceData);
            }

            return resourcesData;
        }

        private async Task<OrderRewardData> CreateOrderRewardData(List<OrderResourceData> resourcesData)
        {
            return new OrderRewardData
            {
                RewardType = CurrencyType.SoftCurrency,
                RewardAmount = GetRewardAmount(resourcesData),
                RewardSprite =
                    await _assetProvider.LoadSprite(
                        _staticDataService.GetCurrencyData(CurrencyType.SoftCurrency).Sprite),
            };
        }

        private int GetRewardAmount(List<OrderResourceData> resourcesData)
        {
            int rewardAmount = 0;

            foreach (OrderResourceData resourceData in resourcesData)
            {
                int resourcePrice = _staticDataService.GetCurrencyCoefficientsData(resourceData.ResourceType)
                    .PriceForOne;
                rewardAmount += (int)(resourceData.Quantity * resourcePrice * OrderGenerateConfigData.RewardRatio);
            }

            return rewardAmount;
        }

        private ResourceType GetRandomResourceType()
        {
            return _resourceOutput.Keys.Count == 0
                ? ResourceType.Oil
                : _resourceOutput.Keys.ToList()[GenerateIntRandom(0, _resourceOutput.Count - 1)];
        }

        private void FillResourcesQuantity()
        {
            Dictionary<ResourceType, float> resourceOutput = new();

            FillResourcesFromProcessingProgress(resourceOutput);

            FillResourcesFromMiningProgress(resourceOutput);

            _resourceOutput = resourceOutput;
        }

        private void FillResourcesFromMiningProgress(Dictionary<ResourceType, float> resourceOutput)
        {
            List<MiningWorkspaceProgress> miningWorkspaceProgresses =
                _progressService.RegionProgress.MiningWorkSpaceProgresses.FindAll(x => x.IsBuilded);

            foreach (MiningWorkspaceProgress mining in miningWorkspaceProgresses)
            {
                MiningWorkspaceStaticData currentProcessingLevelData = _staticDataService
                    .MiningWorkspaceStaticData;


                if (resourceOutput.ContainsKey(currentProcessingLevelData.ResourceType))
                {
                    resourceOutput[currentProcessingLevelData.ResourceType] +=
                        currentProcessingLevelData.MinedResource;
                }
                else
                {
                    resourceOutput.Add(currentProcessingLevelData.ResourceType,
                        currentProcessingLevelData.MinedResource);
                }
            }
        }

        private void FillResourcesFromProcessingProgress(Dictionary<ResourceType, float> resourceOutput)
        {
            List<ProcessingWorkspaceProgress> processingWorkspaceProgresses =
                _progressService.RegionProgress.ProcessingWorkspaceProgresses.FindAll(x => x.IsBuilded);

            foreach (var processing in processingWorkspaceProgresses)
            {
                ProcessingWorkspaceLevel currentProcessingLevelData = _staticDataService
                    .GetProcessingWorkspaceData(processing.Id).BuildingLevels[processing.CurrentLevel];

                foreach (ResourceConversion outputResource in currentProcessingLevelData.ResourceConversionData
                             .OutputResources)
                {
                    if (resourceOutput.ContainsKey(outputResource.ResourceType))
                    {
                        resourceOutput[outputResource.ResourceType] += outputResource.Value;
                    }
                    else
                    {
                        resourceOutput.Add(outputResource.ResourceType, outputResource.Value);
                    }
                }
            }
        }

        private int GetResourceQuantity(ResourceType randomResourceType)
        {
            int resourceQuantity = 1;

            if (_resourceOutput.ContainsKey(randomResourceType))
            {
                resourceQuantity += (int)(_resourceOutput[randomResourceType] * GenerateFloatRandom
                    (OrderGenerateConfigData.MinProductQuantity, OrderGenerateConfigData.MaxProductQuantity));
            }

            return resourceQuantity;
        }

        private async Task CheckSpriteOnNull(OrderProgress orderProgress)
        {
            if (orderProgress.RewardsData.RewardSprite == null || orderProgress.RewardsData.RewardSprite.name == "")
            {
                orderProgress.RewardsData.RewardSprite = await _assetProvider.LoadSprite(_staticDataService
                    .GetCurrencyData(orderProgress.RewardsData.RewardType).Sprite);

                foreach (OrderResourceData resource in orderProgress.ResourcesData)
                {
                    resource.ResourceSprite = await _assetProvider.LoadSprite(_staticDataService
                        .GetResourceStaticData(resource.ResourceType).SpriteAssetReference);
                }
            }
        }

        private int GenerateIntRandom(int min, int max)
        {
            return Random.Range(min, max);
        }

        private float GenerateFloatRandom(float min, float max)
        {
            return Random.Range(min, max);
        }
    }
}