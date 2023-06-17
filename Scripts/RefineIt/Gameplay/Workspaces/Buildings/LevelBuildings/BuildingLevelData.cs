using System;
using Gameplay.Currencies;
using UnityEngine.AddressableAssets;

namespace Gameplay.Workspaces.Buildings.LevelBuildings
{
    [Serializable]
    public abstract class BuildingLevelData
    {
        public AssetReferenceSprite SpriteView;
        public CurrencyType CostType;
        public int UpdateCost;
        public int Buildings;
    }
}