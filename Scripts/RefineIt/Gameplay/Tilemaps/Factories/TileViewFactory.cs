using Gameplay.Tilemaps.AnimatedTiles;
using Gameplay.Tilemaps.Services;
using Gameplay.Tilemaps.Views;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.Factories
{
    public class TileViewFactory : ITileViewFactory
    {
        private Tilemap _interactableTilemap;
        private Tilemap _animTilemap1;
        private Tilemap _animTilemap2;
        private readonly ICoroutineService _coroutineService;

        public TileViewFactory(ICoroutineService coroutineService, ITileStorage tileStorage)
        {
            _coroutineService = coroutineService;
        }

        public void Initialize()
        {
            var tilemapMarker = Object.FindObjectOfType<TilemapMarker>();
            _interactableTilemap = tilemapMarker.InteractableTilemap;
            _animTilemap1 = tilemapMarker.AnimationTilemap1;
            _animTilemap2 = tilemapMarker.AnimationTilemap2;
        }

        public void SetTile(Vector3Int position, TileBase tile)
        {
            _interactableTilemap.SetTile(position, tile);
        }

        public BuildingTileView Create(Vector3Int position)
        {
            var fadeAnimationTile1 = new FadeAnimationTile(_animTilemap1, _coroutineService);
            var fadeAnimationTile2 = new FadeAnimationTile(_animTilemap2, _coroutineService);
            var fadeAnimationTiles = new FadeAnimationTiles(fadeAnimationTile1, fadeAnimationTile2);
            return new BuildingTileView(_interactableTilemap, position, fadeAnimationTiles);
        }
    }
}