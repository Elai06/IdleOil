using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Workspaces.ComplexWorkspace;
using Infrastructure.StaticData.ProcessingWorkspace;
using Sirenix.OdinInspector;

namespace Gameplay.Tilemaps.Data
{
    [Serializable]
    public class BuildingTileData
    {
        [ValueDropdown(nameof(GetBuildingIds))] public string Id;
        public BuildingType Type;
        public BuildingLevelTileData[] Levels;

        private IEnumerable GetBuildingIds()
        {
            switch(Type)
            {
                case BuildingType.Mining:
                    return new List<string>() { BuildingType.Mining.ToString() };
                case BuildingType.Complex:
                    return Enum.GetNames(typeof(ComplexType));
                case BuildingType.Process:
                    return Enum.GetNames(typeof(ProcessingType));
                case BuildingType.Storage:
                    return new List<string>() { BuildingType.Storage.ToString() };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}