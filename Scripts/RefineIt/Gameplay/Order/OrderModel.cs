using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Orders;
using Gameplay.Quests;
using Gameplay.Region.Storage;
using Gameplay.Services.Timer;
using Infrastructure.PersistenceProgress;
using Infrastructure.YandexAds;
using UnityEngine;

namespace Gameplay.Order
{
    [Serializable]
    public class OrderModel
    {
        public event Action<OrderModel> UpdateOrderStatus;
        public event Action<OrderModel> Tick;

        public int ID;
        public List<OrderResourceData> ResourcesData;
        public OrderRewardData RewardsData;
        public OrderProgress OrderProgress;
        public bool IsTimer;

        private IRegionStorage _regionStorage;
        private CurrenciesModel _currenciesModel;
        private TimerService _timerService;
        private IAdsService _adsService;
        private IQuestModel _questModel;

        public TimeModel Timer;

        public OrderModel(IRegionStorage regionStorage, CurrenciesModel currenciesModel, TimerService timerService,
            OrderProgress orderProgress, IAdsService adsService, IQuestModel questModel, bool isTimer = true)
        {
            _regionStorage = regionStorage;
            _currenciesModel = currenciesModel;
            _timerService = timerService;
            _adsService = adsService;

            OrderProgress = orderProgress;
            ID = orderProgress.ID;
            ResourcesData = orderProgress.ResourcesData;
            RewardsData = orderProgress.RewardsData;
            IsTimer = isTimer;
            _questModel = questModel;

            CreateTimer();
        }

        public Sprite ClientSprite;

        public void CreateTimer()
        {
            if (!IsTimer) return;
            Timer = _timerService.CreateTimer($"{TimerType.OrderToFailed}{ID}", OrderProgress.Time);
            Timer.IsWork = OrderProgress.OrderStatus == OrderStatus.Working ||
                           OrderProgress.OrderStatus == OrderStatus.AvailableReward;

            Timer.Tick += OnTick;
            Timer.Stopped += StopTimer;
        }

        private void StopTimer(TimeModel timer)
        {
            Timer.Tick -= OnTick;
            Timer.Stopped -= StopTimer;

            ChangeOrderStatus(OrderStatus.Failed);
        }

        private void OnTick(int time)
        {
            Tick?.Invoke(this);
            OrderProgress.Time = time;
        }

        public void GetReward()
        {
            if (OrderProgress.OrderStatus == OrderStatus.AvailableReward)
            {
                _currenciesModel.Add(RewardsData.RewardType, RewardsData.RewardAmount);
                _questModel.TaskDailyProgress(QuestsGuid.performOffer, 1);
                _questModel.TaskWeeklyProgress(QuestsGuid.performOfferWeek, 1);

                if (IsTimer)
                {
                    Timer.Tick -= OnTick;
                    Timer.Stopped -= StopTimer;
                }

                ChangeOrderStatus(OrderStatus.Complete);
            }
        }

        public void TakeOrder()
        {
            if (OrderProgress.OrderStatus == OrderStatus.Idle)
            {
                ChangeOrderStatus(OrderStatus.Working);

                if (IsTimer)
                    Timer.IsWork = true;
            }
        }

        public bool ChangeOrderStatus(OrderStatus orderStatus)
        {
            if (OrderProgress.OrderStatus == orderStatus)
                return false;
            
            OrderProgress.OrderStatus = orderStatus;
            
            if (orderStatus == OrderStatus.AvailableReward)
            {
                foreach (var resource in ResourcesData)
                {
                    _regionStorage.TakeResources(resource.ResourceType, resource.Quantity);
                }

                return true;
            }
            
            UpdateOrderStatus?.Invoke(this);
            return true;
        }

        public void ShowAds()
        {
            if (OrderProgress.isCanShowAds)
            {
                Timer.Tick -= OnTick;
                Timer.Stopped -= StopTimer;
                
                _adsService.VideoShown += OnChangeStatus;
                _adsService.ShowAdsVideo();
            }
        }

        private void OnChangeStatus()
        {
            _adsService.VideoShown -= OnChangeStatus;
            ChangeOrderStatus(OrderStatus.AdsReplace);
            OrderProgress.isCanShowAds = false;
        }
    }
}