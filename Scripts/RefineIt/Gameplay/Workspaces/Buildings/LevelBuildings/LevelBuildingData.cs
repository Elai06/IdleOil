using System.Collections.Generic;

namespace Gameplay.Workspaces.Buildings.LevelBuildings
{
    public abstract class LevelBuildingData<TBuildingLevelData> : BuildingStaticData where TBuildingLevelData : BuildingLevelData
    {
        public List<TBuildingLevelData> BuildingLevels;
    }
}