using Gameplay.Tilemaps.AnimatedTiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.Views
{
    public class BuildingTileView
    {
        private readonly Tilemap _tilemap;
        private readonly FadeAnimationTiles _fadeAnimationTiles;
        private Vector3Int _tilePosition;
        
        public BuildingTileView(Tilemap tilemap, Vector3Int tilePosition, FadeAnimationTiles fadeAnimationTiles)
        {
            _fadeAnimationTiles = fadeAnimationTiles;
            _tilemap = tilemap;
            _tilePosition = tilePosition;
        }
        
        public void SetTile(TileBase tileBase)
        {
            _tilemap.SetTile(_tilePosition, tileBase);
        }
        
        public void SetTile(TileBase tileBase, Vector3Int tilePosition)
        {
            _tilePosition = tilePosition;
            _tilemap.SetTile(tilePosition, tileBase);
        }

        public void SetAnimationTiles(TileBase animationTile1, TileBase animationTile2)
        {
            _fadeAnimationTiles.InitializeFirstTile(_tilePosition, animationTile1);
            _fadeAnimationTiles.InitializeSecondTile(_tilePosition, animationTile2);
            _fadeAnimationTiles.StartAnimation();
        }

        public void DisableAnimation()
        {
            _fadeAnimationTiles.StopAnimation();
        }
    }
}