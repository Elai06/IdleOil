using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.AnimatedTiles
{
    public class FadeAnimationTiles
    {
        private readonly FadeAnimationTile _animationTile1;
        private readonly FadeAnimationTile _animationTile2;

        public FadeAnimationTiles(FadeAnimationTile animationTile1, FadeAnimationTile animationTile2)
        {
            _animationTile1 = animationTile1;
            _animationTile2 = animationTile2;
        }


        public void InitializeFirstTile(Vector3Int position, TileBase tileBase)
        {
            _animationTile1.Initialize(position, tileBase);
        }

        public void InitializeSecondTile(Vector3Int position, TileBase tileBase)
        {
            _animationTile2.Initialize(position, tileBase);
        }
        
        public void StartAnimation()
        {
            _animationTile1.FadePingPong(1f, 0f, 1f);
            _animationTile2.FadePingPong(0f, 1f, 1f);
        }

        public void StopAnimation()
        {
            _animationTile1.Stop();
            _animationTile2.Stop();
        }
    }
}