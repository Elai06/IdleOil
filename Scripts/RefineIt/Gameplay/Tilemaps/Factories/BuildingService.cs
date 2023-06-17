using System;
using System.Collections.Generic;
using Gameplay.Tilemaps.Buildings;
using UnityEngine;

namespace Gameplay.Tilemaps.Factories
{
    public class BuildingService : IBuildingService
    {
        private readonly Dictionary<Vector3Int, IBuilding> _buildings = new Dictionary<Vector3Int, IBuilding>();

        public IEnumerable<IBuilding> Buildings => _buildings.Values;

        public void AddBuilding(IBuilding building)
        {
            _buildings.Add(building.Guid, building);
        }

        public IBuilding GetBuilding(Vector3Int guid)
        {
            if(_buildings.TryGetValue(guid, out var building))
                return building;

            throw new InvalidOperationException($"Building with Guid {guid}, doesn't stored inf BuildingService");
        }
    }
}