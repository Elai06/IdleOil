using System;
using Gameplay.Tilemaps.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.Tilemaps.Services
{
    [CreateAssetMenu(fileName = "TilesContainer", menuName = "Data/Tiles/TileContainer")]
    public class TilesContainer : ScriptableObject
    {
        public AnimatedTileData Foundation;
        public AnimatedTileData[] Constructions;
        public BuildingTileData[] Buildings;

        [Header("Pipelines")] 
        public BuildingPipeTile ImportPipeTile;
        public BuildingPipeTile ExportPipeTile;
        
        public PipelineTileData[] Pipelines;
    }

    [Serializable]
    public class BuildingPipeTile
    {
        public Vector3Int BuildingOffset;
        public TileBase Tile;
    }

    [Serializable]
    public class PipelineTileData
    {
        public string PipeType;
        public TileBase PipeTile;
    }
}