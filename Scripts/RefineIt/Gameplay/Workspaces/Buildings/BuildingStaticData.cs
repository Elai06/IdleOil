using Gameplay.Currencies;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Workspaces.Buildings
{
    public abstract class BuildingStaticData : ScriptableObject
    {
        public AssetReferenceSprite SpriteView;
        public float BuildTime;
        public int Cost;
        public CurrencyType CostType;
    }
}