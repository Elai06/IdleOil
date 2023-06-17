using System;
using Gameplay.Currencies;
using UnityEngine;

namespace Gameplay.Orders
{
    [Serializable]
    public class OrderRewardData
    {
        public CurrencyType RewardType;
        public Sprite RewardSprite;
        public int RewardAmount;
    }
}