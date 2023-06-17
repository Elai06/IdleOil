using System;
using System.Collections.Generic;
using Gameplay.Orders;
using Gameplay.Services.Timer;

namespace Gameplay.Order
{
    [Serializable]
    public class OrderProgress
    {
        public int ID;
        public OrderStatus OrderStatus;
        public List<OrderResourceData> ResourcesData;
        public OrderRewardData RewardsData;
        public int ClientSpriteIndex;
        public int Time;
        public bool isCanShowAds;

        public OrderProgress(OrderStatus orderStatus, int id, OrderRewardData rewardsData, List<OrderResourceData> resourcesData)
        {
            OrderStatus = orderStatus;
            ID = id;
            RewardsData = rewardsData;
            ResourcesData = resourcesData;
        }
    }

    public enum OrderStatus
    {
        Idle,
        Working,
        AvailableReward,
        Complete,
        Failed,
        AdsReplace
    }
}