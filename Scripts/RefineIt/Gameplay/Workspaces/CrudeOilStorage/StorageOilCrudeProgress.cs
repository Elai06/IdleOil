using System;
using Gameplay.Workspaces.Buildings.LevelBuildings;
using UnityEngine;

namespace Gameplay.Workspaces.CrudeOilStorage
{
    [Serializable]
    public class StorageOilCrudeProgress : LevelBuildingProgress
    {
        public StorageOilCrudeProgress(Vector3Int guid)
        {
            Guid = guid;
        }
    }
}