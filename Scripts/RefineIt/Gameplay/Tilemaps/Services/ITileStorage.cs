using System.Collections.Generic;
using Gameplay.Region.Data;
using Gameplay.Tilemaps.Data;
using Gameplay.Workspaces.Buildings.States;

namespace Gameplay.Tilemaps.Services
{
    public interface ITileStorage
    {
        void Load(RegionType regionType);
        AnimatedTileData GetBuildingTilesData(string buildingId, int level = 0);
        bool HasTiles(string buildingId, int level = 0);
        AnimatedTileData GetFoundationTilesData();
        TileMapData TileMapData { get; }
        PipelineTileData[] PipelineData { get; }
        BuildingPipeTile ExportPipelineData { get; }
        BuildingPipeTile ImportPipelineData { get; }
        Dictionary<string, BuildingTileData> BuildingTiles { get; }
        AnimatedTileData GetConstructionTilesData(int level);
    }
}