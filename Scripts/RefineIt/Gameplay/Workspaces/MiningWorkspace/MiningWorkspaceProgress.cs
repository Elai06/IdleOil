using System;
using Gameplay.Workspaces.Buildings;
using UnityEngine;

namespace Gameplay.Workspaces.MiningWorkspace
{
    [Serializable]
    public class MiningWorkspaceProgress : BuildingProgress
    {
        public MiningWorkspaceProgress(Vector3Int guid)
        {
            Guid = guid;
        }
    }
}