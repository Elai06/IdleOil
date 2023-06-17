using System;
using Gameplay.Workspaces.Buildings.LevelBuildings;
using Gameplay.Workspaces.ComplexWorkspace;

namespace Infrastructure.StaticData.ProcessingWorkspace
{
    [Serializable]
    public class ProcessingWorkspaceLevel : BuildingLevelData
    {
        public ResourceCapacity[] RequiredStorageCapacity;
        public ResourceCapacity[] ProduceStorageCapacity;

        public ResourcesConversionData ResourceConversionData;
        
        public int ProcessingTime;
        public string Description;
    }
}