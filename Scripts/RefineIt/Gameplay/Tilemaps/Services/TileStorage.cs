using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Region.Data;
using Gameplay.Tilemaps.Data;
using Gameplay.Workspaces.Pipes;
using UnityEngine;

namespace Gameplay.Tilemaps.Services
{
    public class TileStorage : ITileStorage
    {
        private readonly Dictionary<string, BuildingTileData> _buildingTiles =
            new Dictionary<string, BuildingTileData>();

        private AnimatedTileData[] _constructionTiles;
        private AnimatedTileData _foundationTileData;

        public TileMapData TileMapData { get; private set; }
        public PipelineTileData[] PipelineData { get; private set; }
        public BuildingPipeTile ImportPipelineData { get; private set; }
        public BuildingPipeTile ExportPipelineData { get; private set; }

        public Dictionary<string, BuildingTileData> BuildingTiles => _buildingTiles;

        public void Load(RegionType regionType)
        {
            TileMapData = Resources.Load<TileMapData>($"Configs/Region/{regionType.ToString()}/Tiles/TilemapData");
            var foundationTileData =
                Resources.Load<TilesContainer>($"Configs/Region/{regionType.ToString()}/Tiles/TilesContainer");
            ExportPipelineData = foundationTileData.ExportPipeTile;
            ImportPipelineData = foundationTileData.ImportPipeTile;
            PipelineData = foundationTileData.Pipelines;
            _foundationTileData = foundationTileData.Foundation;
            foreach (var buildingTileData in foundationTileData.Buildings)
                _buildingTiles.Add(buildingTileData.Id, buildingTileData);
            _constructionTiles = foundationTileData.Constructions;
        }

        public AnimatedTileData GetBuildingTilesData(string buildingId, int level = 0)
        {
            foreach (var buildingLevelTileData in _buildingTiles[buildingId].Levels)
            {
                if (buildingLevelTileData.Level == level)
                    return buildingLevelTileData.AnimationTileData;
            }

            throw new InvalidOperationException(
                $"BuildingLevelTileData with id:{buildingId} doesn't have data for level {level}");
        }

        public bool HasTiles(string buildingId, int level = 0)
        {
            if (_buildingTiles.TryGetValue(buildingId, out var buildingTileData))
                return buildingTileData.Levels.Any(levelTileData => levelTileData.Level == level);
            return false;
        }

        public AnimatedTileData GetConstructionTilesData(int level)
        {
            if (level < 0 || level >= _constructionTiles.Length)
                throw new InvalidOperationException($"Tile storage doesn't contains Tile for Level : {level}");

            return _constructionTiles[level];
        }

        public AnimatedTileData GetFoundationTilesData() =>
            _foundationTileData;
    }
}