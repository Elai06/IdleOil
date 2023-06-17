using Gameplay.Tilemaps.Data;

namespace Gameplay.Tilemaps.Services
{
    public interface ITileSpawner
    {
        void Spawn(BuildingData buildingData);
    }
}