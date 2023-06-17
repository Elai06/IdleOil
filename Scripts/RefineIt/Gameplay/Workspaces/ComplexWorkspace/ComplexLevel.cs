using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Workspaces.MiningWorkspace;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    [Serializable]
    public struct ComplexLevel
    {
        public AssetReferenceGameObject ViewComplexZone;
        public AssetReferenceSprite SpriteView;
        public int Level;
        public CurrencyType CostType;
        public int Cost;
        public List<ResourceCapacity> ResourcesCapacity;
    }
}