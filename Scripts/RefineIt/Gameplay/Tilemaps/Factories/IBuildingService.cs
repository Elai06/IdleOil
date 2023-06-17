using System.Collections.Generic;
using Gameplay.Tilemaps.Buildings;
using UnityEngine;

namespace Gameplay.Tilemaps.Factories
{
    public interface IBuildingService
    {
        IEnumerable<IBuilding> Buildings { get; }
        void AddBuilding(IBuilding building);
        IBuilding GetBuilding(Vector3Int guid);
    }
}