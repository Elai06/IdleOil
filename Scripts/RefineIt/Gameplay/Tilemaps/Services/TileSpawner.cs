using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Data;
using Gameplay.Tilemaps.Factories;

namespace Gameplay.Tilemaps.Services
{
    public class TileSpawner : ITileSpawner
    {
        private readonly IBuildingFactory _buildingFactory;
        private readonly ITilemapController _tilemapController;


        public TileSpawner(IBuildingFactory buildingFactory,
            ITilemapController tilemapController)
        {
            _buildingFactory = buildingFactory;
            _tilemapController = tilemapController;
        }

        public void Spawn(BuildingData buildingData)
        {
            var building = _buildingFactory.Create(buildingData);
            _tilemapController.Add(building);
        }
    }
}