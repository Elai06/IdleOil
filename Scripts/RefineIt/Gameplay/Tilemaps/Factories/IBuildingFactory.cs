using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Data;

namespace Gameplay.Tilemaps.Factories
{
    public interface IBuildingFactory
    {
        IBuilding Create(BuildingData buildingData);
    }
}