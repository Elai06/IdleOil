using System;
using System.Collections;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.AnimatedTiles
{
    public class FadeAnimationTile
    {
        private readonly Tilemap _animationTilemap;
        private readonly ICoroutineService _coroutineService;
        
        private Vector3Int _position;
        private Coroutine _coroutine;

        public FadeAnimationTile(Tilemap animationTilemap, ICoroutineService coroutineService)
        {
            _animationTilemap = animationTilemap;
            _coroutineService = coroutineService;
        }

        public void Initialize(Vector3Int position, TileBase tile)
        {
            _position = position;
            _animationTilemap.SetTile(position, tile);
        }

        public void FadePingPong(float from, float to, float time)
        {
            if(_coroutine != null)
                _coroutineService.StopCoroutine(_coroutine);
            
            var color = new Color(1, 1, 1, from);
            _coroutine = _coroutineService.StartCoroutine(PingPongTileAlpha(color, from, to, time));
        }

        private IEnumerator PingPongTileAlpha(Color color, float from, float to, float time)
        {
            var currentTime = 0f;
            var currentAlpha = from;
            while(Math.Abs(currentAlpha - to) > 0.001f)
            {
                currentTime += Time.deltaTime;
                currentAlpha = Mathf.Lerp(from, to, currentTime / time);
                color.a = currentAlpha;
                _animationTilemap.SetColor(_position, color);
                yield return null;
            }
            _coroutine = _coroutineService.StartCoroutine(PingPongTileAlpha(color, to, from, time));
        }

        public void Stop()
        {
            if(_coroutine != null)
                _coroutineService.StopCoroutine(_coroutine);
            _animationTilemap.SetColor(_position, new Color(1f,1f,1f,0f));
        }
    }
}