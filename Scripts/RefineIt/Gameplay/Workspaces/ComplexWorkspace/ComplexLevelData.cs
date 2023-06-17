using System;
using System.Collections.Generic;
using Gameplay.Workspaces.Buildings;
using Gameplay.Workspaces.Buildings.LevelBuildings;
using UnityEngine.AddressableAssets;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    [Serializable]
    public class ComplexLevelData : BuildingLevelData
    {
        public List<ResourceCapacity> ResourcesCapacity;
    }
}