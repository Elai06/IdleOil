using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Tilemaps.Data
{
    [Serializable]
    public partial class BuildingData
    {
        public bool BuildFromStart;
        public string Id;
        public Vector3Int Position;
        public BuildingType Type;
        public List<AdditionalBuilding> AdditionalBuildings;

        public BuildingData(string id, Vector3Int position, BuildingType type, List<AdditionalBuilding> additionalBuildings)
        {
            Id = id;
            Position = position;
            Type = type;
            AdditionalBuildings = additionalBuildings;
        }
    }
}