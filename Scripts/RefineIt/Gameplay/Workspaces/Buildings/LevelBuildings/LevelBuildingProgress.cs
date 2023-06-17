using System;

namespace Gameplay.Workspaces.Buildings.LevelBuildings
{
    [Serializable]
    public abstract class LevelBuildingProgress : BuildingProgress
    {
        public int CurrentLevel;
    }
}