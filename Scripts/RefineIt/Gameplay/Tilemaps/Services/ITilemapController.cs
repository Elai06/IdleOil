using System;
using Gameplay.Tilemaps.Buildings;
using UnityEngine;

namespace Gameplay.Tilemaps.Services
{
    public interface ITilemapController
    {
        void Initialize();
        void Add(IBuilding building);
        void Cleanup();
        event Action<Vector3Int> ClickOnBuild;
    }
}