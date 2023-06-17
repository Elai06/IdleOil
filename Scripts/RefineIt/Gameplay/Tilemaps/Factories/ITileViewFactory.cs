using Gameplay.Tilemaps.Views;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.Factories
{
    public interface ITileViewFactory
    {
        void Initialize();
        BuildingTileView Create(Vector3Int position);
        void SetTile(Vector3Int position, TileBase tile);
    }
}