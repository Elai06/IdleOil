using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps
{
    public class TilemapMarker : MonoBehaviour
    {
        [field: SerializeField] public Grid Grid { get; private set; }
        [field: SerializeField] public Tilemap InteractableTilemap { get; private set; }
        [field: SerializeField] public Tilemap AnimationTilemap2 { get; private set; }
        [field: SerializeField] public Tilemap AnimationTilemap1 { get; private set; }
      //  [field: SerializeField] public Tilemap PipesTile { get; private set; }
        [field: SerializeField] public Tilemap RailRoads { get; private set; }
    }
}