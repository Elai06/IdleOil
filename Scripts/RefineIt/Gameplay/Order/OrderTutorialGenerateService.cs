using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay.Currencies;
using Gameplay.Orders;
using Gameplay.Quests;
using Gameplay.Region.Storage;
using Gameplay.Services.Timer;
using Infrastructure.AssetManagement;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.YandexAds;
using UnityEngine;
using Zenject;

namespace Gameplay.Order
{
    public class OrderTutorialGenerateService
    {
        public event Action<OrderModel> CreatedOrder;
        public event Action<OrderModel> UpdateOrder;
        public event Action<OrderModel> RemoveOrderInDictionary;

        private IStaticDataService _staticDataService;
        private IProgressService _progressService;
        private IAssetProvider _assetProvider;
        private IRegionStorage _regionStorage;
        private CurrenciesModel _currenciesModel;
        private TimerService _timerService;
        private OrderGenerateService _orderGenerateService;
        private IAdsService _adsServiceService;
        private IQuestModel _questModel;

        [Inject]
        public void Construct(IStaticDataService staticDataService, IProgressService progressService,
            IAssetProvider assetProvider, IRegionStorage regionStorage, CurrenciesModel currenciesModel,
            TimerService timerService, OrderGenerateService orderGenerateService, IAdsService adsServiceService,
            IQuestModel questModel)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            _assetProvider = assetProvider;
            _regionStorage = regionStorage;
            _currenciesModel = currenciesModel;
            _timerService = timerService;
            _orderGenerateService = orderGenerateService;
            _adsServiceService = adsServiceService;
            _questModel = questModel;
        }

        public List<OrderConfigData> OrderTutorialConfigData => _staticDataService.OrderTutorialConfig.OrderConfigsData;
        public OrderGenerateConfigData OrderConfigData => _staticDataService.OrderConfigData.Data;

        public void Initialize()
        {
            if (_progressService.RegionProgress.IsTutorial)
            {
                CreateOrderModels();
            }
        }

        private void CreateOrderModels()
        {
            for (int i = 0; i < OrderConfigData.OrderCount; i++)
            {
                GenerateOrder(i);
            }
        }

        private async void GenerateOrder(int index)
        {
            if (_progressService.RegionProgress.OrdersProgress.OrderProgresses.Count < index + 1)
            {
                if (_progressService.RegionProgress.IsLastOrderTutorial)
                {
                    return;
                }
            }

            OrderProgress orderProgress = _progressService.RegionProgress.OrdersProgress.OrderProgresses.Count > index
                ? _progressService.RegionProgress.OrdersProgress.OrderProgresses[index]
                : _progressService.RegionProgress.OrdersProgress.GetOrCreate(index);

            TimeProgress timerProgress = _progressService.RegionProgress.TimeOrderProgresses
                .GetOrCreateTimeProgress($"{TimerType.CreatedOrder}{orderProgress.ID}", 0);

            int delay = timerProgress?.Time ?? 0;

            if (delay > 0)
            {
                TimeModel timeModel = _timerService.CreateTimer($"{TimerType.CreatedOrder}{orderProgress.ID}", delay);

                timeModel.Stopped += CreateOrderLater;
                return;
            }

            CreateOrder(orderProgress.ID);
        }

        private async void CreateOrder(int index)
        {
            OrderProgress orderProgress = _progressService.RegionProgress.OrdersProgress.OrderProgresses
                .Find(x => x.ID == index);

            List<OrderResourceData> resourcesData = OrderTutorialConfigData[orderProgress.ID].ResourcesData;

            OrderRewardData orderRewardData = await CreateOrderRewardData(resourcesData);

            orderProgress.ResourcesData = resourcesData;
            orderProgress.RewardsData = orderRewardData;
            orderProgress.Time = OrderConfigData.TimeToFailed;
            orderProgress.isCanShowAds = false;
            
            OrderModel orderModel = new(_regionStorage, _currenciesModel, _timerService, orderProgress,
                _adsServiceService, _questModel, false);

            _orderGenerateService.OrderModels.Add(orderProgress.ID, orderModel);
            
            _orderGenerateService.OrderModels[orderProgress.ID].ClientSprite = await _orderGenerateService.GetClientSprite(orderProgress);

            CreatedOrder?.Invoke(orderModel);
            orderModel.UpdateOrderStatus += OnUpdateOrderStatus;
        }

        private void CreateOrderLater(TimeModel timeModel)
        {
            string id = timeModel.TimeProgress.ID.Remove(0, timeModel.TimeProgress.ID.Length - 1);

            CreateOrder(int.Parse(id));

            timeModel.Stopped -= CreateOrderLater;
        }

        private void CreateNewOrderLater(int id)
        {
            string timeId = $"{TimerType.CreatedOrder}{id}";

            int delay = 15; /*Random.Range(FormatTime.MinutesIntFormat(OrderConfigData.MinDelayNextOrder),
                FormatTime.MinutesIntFormat(OrderConfigData.MaxDelayNextOrder));*/

            _progressService.RegionProgress.OrdersProgress.GetOrCreate(id);

            TimeModel timeModel = _timerService.CreateTimer(timeId, delay);
            timeModel.Stopped += CreateOrderLater;

            if (id + 1 >= OrderTutorialConfigData.Count)
            {
                _progressService.RegionProgress.IsLastOrderTutorial = true;
            }
        }

        private void OnUpdateOrderStatus(OrderModel orderModel)
        {
            if (orderModel.OrderProgress.OrderStatus == OrderStatus.Complete)
            {
                if (_progressService.RegionProgress.IsLastOrderTutorial
                    && _progressService.RegionProgress.OrdersProgress.OrderProgresses.Count != 0)
                {
                    RemoveOrder(orderModel);

                    if (_progressService.RegionProgress.OrdersProgress.OrderProgresses.Count == 0)
                    {
                        _progressService.RegionProgress.IsTutorial = false;

                        _orderGenerateService.Initialize();
                    }

                    return;
                }

                CreateNewOrderLater(_progressService.RegionProgress.OrdersProgress.OrderProgresses[^1].ID + 1);
                RemoveOrder(orderModel);
                return;
            }

            UpdateOrder?.Invoke(orderModel);
        }

        private void RemoveOrder(OrderModel orderModel)
        {
            _timerService.TimeModels.Remove($"{TimerType.CreatedOrder}{orderModel.ID}");
            _progressService.RegionProgress.OrdersProgress.OrderProgresses.Remove(orderModel.OrderProgress);
            _orderGenerateService.OrderModels.Remove(orderModel.ID);
            RemoveOrderInDictionary?.Invoke(orderModel);
        }

        private async Task<OrderRewardData> CreateOrderRewardData(List<OrderResourceData> resourcesData)
        {
            return new OrderRewardData
            {
                RewardType = CurrencyType.SoftCurrency,
                RewardAmount = GetRewardAmount(resourcesData),
                RewardSprite = await _assetProvider.LoadSprite(
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
                rewardAmount += (int)(resourceData.Quantity * resourcePrice * OrderConfigData.RewardRatio);
            }

            return rewardAmount;
        }
    }
}