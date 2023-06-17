using System;
using UnityEngine;

namespace Gameplay.Tilemaps.Services
{
    public interface ITileClickHandler
    {
        event Action<Vector3Int> Clicked;
        void Initialize();
    }
}