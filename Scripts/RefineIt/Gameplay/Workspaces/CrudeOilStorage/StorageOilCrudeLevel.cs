using System;
using System.Collections.Generic;
using Gameplay.Workspaces.Buildings;
using Gameplay.Workspaces.Buildings.LevelBuildings;
using Gameplay.Workspaces.ComplexWorkspace;

namespace Gameplay.Workspaces.CrudeOilStorage
{
    [Serializable]
    public class StorageOilCrudeLevel : BuildingLevelData
    {
        public List<ResourceCapacity> ResourcesCapacity;
    }
}