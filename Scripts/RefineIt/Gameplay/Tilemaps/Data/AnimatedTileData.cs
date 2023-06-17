using System;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.Data
{
    [Serializable]
    public class AnimatedTileData
    {
        public TileBase DefaultTile;
        public TileBase AnimatedTile1;
        public TileBase AnimatedTile2;
    }
}