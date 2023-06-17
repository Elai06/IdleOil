using Gameplay.Workspaces.Buildings.LevelBuildings;
using UnityEngine;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    [CreateAssetMenu(fileName = "ComplexWorkspace", menuName = "Data/ComplexWorkspaceStaticData")]
    public class ComplexWorkspaceStaticData : LevelBuildingData<ComplexLevelData>
    {
        public ComplexType ComplexType;
        public string Description;
    }
}