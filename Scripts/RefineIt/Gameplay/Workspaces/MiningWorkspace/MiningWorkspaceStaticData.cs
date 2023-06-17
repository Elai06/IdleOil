using Gameplay.Workspaces.Buildings;
using UnityEngine;

namespace Gameplay.Workspaces.MiningWorkspace
{
    [CreateAssetMenu(fileName = "MiningWorkspace", menuName = "Data/MiningWorkspace")]
    public class MiningWorkspaceStaticData : BuildingStaticData
    {
        public ResourceType ResourceType;
        public int MinedResource;
        public float MiningDuration;
        public string Description;
    }
}