using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Workspaces.MiningWorkspace;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Workers
{
    [CreateAssetMenu(fileName = "WorkersConfig", menuName = "Data/WorkersConfig")]
    public class WorkersConfig : ScriptableObject
    {
        public List<WorkerData> WorkersData;
    }

    [Serializable]
    public class WorkerData
    {
        public WorkerType WorkerType;
        public CurrencyType CurrencyType;
        public AssetReferenceSprite WorkerSpriteReference;
        public ResourceType ResourceType;
        public List<WorkerLevelData> WorkerLevelsData;
    }

    [Serializable]
    public class WorkerLevelData
    {
        public int Price;
        [Range(0, 100)] public int Bonus;
    }

    [Serializable]
    public enum WorkerType
    {
        Max,
        Leo,
        Chloe,
        Jason,
        Lucas,
        Stella,
        James,
        Mia,
        Calvin,
        Miles,
        Kevin,
        Ava,
        Tyler,
        Emily


    }
}
