using Gameplay.Workspaces.Buildings.LevelBuildings;
using UnityEngine;

namespace Infrastructure.StaticData.ProcessingWorkspace
{
    [CreateAssetMenu(fileName = "ProcessingWorkspace", menuName = "Data/ProcessingWorkspaceStaticData")]
    public class ProcessingWorkspaceStaticData : LevelBuildingData<ProcessingWorkspaceLevel>
    {
        public ProcessingType Type;
    }
}