using System;

namespace Gameplay.Tilemaps.Buildings
{
    public interface ILevelsBuilding
    {
        event Action<ILevelsBuilding> LevelUpdated;
        int CurrentLevel { get; }
        int NumbersBuildings { get; }
    }
}