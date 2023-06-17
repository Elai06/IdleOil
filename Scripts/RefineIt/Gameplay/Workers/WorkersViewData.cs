using UnityEngine;

namespace Gameplay.Workers
{
    public class WorkersViewData
    {
        public string Name;
        public string Level;
        public string Price;
        
        public string BonusValue;
        public string NextBonusValue;

        public bool IsBuy;
        public bool IsAvailable;
        public bool IsMaxLevel;

        public Sprite WorkerImage;
        public Sprite ResourceImage;
        public Sprite PriceImage;
    }
}