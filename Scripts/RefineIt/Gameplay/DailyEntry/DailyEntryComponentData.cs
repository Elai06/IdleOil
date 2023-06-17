using System;
using Gameplay.Currencies;
using UnityEngine;

namespace Gameplay.DailyEntry
{
    [Serializable]
    public struct DailyEntryComponentData
    {
        public DailyEntryType Day;
        public CurrencyType rewardType;
        public string reward;
        public Sprite sprite;
        public string description;
        public int CircleTake;
    }
}