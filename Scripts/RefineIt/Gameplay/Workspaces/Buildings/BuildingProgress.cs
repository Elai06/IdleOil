using System;
using UnityEngine;

namespace Gameplay.Workspaces.Buildings
{
    [Serializable]
    public abstract class BuildingProgress
    {
        public bool IsBuilded;
        public float ConstructionProgress;
        public Vector3Int Guid;
    }
}