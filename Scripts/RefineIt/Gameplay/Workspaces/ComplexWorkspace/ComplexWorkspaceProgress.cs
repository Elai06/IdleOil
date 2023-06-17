using System;
using Gameplay.Workspaces.Buildings;
using Gameplay.Workspaces.Buildings.LevelBuildings;
using UnityEngine;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    [Serializable]
    public class ComplexWorkspaceProgress : LevelBuildingProgress
    {
        public ComplexType ComplexType;
        public string Id;

        public ComplexWorkspaceProgress(Vector3Int guid)
        {
            Guid = guid;
        }
    }
}