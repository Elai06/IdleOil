using System.Collections.Generic;
using System.Linq;
using Gameplay.Currencies;
using Gameplay.Quests;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.Buildings.LevelBuildings;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    public class ComplexWorkspaceModel : LevelBuildingBase<ComplexWorkspaceProgress, ComplexWorkspaceStaticData, ComplexLevelData>, IImportStorage, IExportStorage
    {
        private readonly IRegionStorage _regionStorage;
        
        public ComplexWorkspaceModel(ComplexWorkspaceProgress progress, ComplexWorkspaceStaticData data,
            CurrenciesModel currenciesModel, IRegionStorage regionStorage, IQuestModel questModel) : base(progress, data, currenciesModel, questModel)
        {
            _regionStorage = regionStorage;
        }

        public override string Id => Data.ComplexType.ToString();
        public ComplexType ComplexType => Data.ComplexType;
        
        public IReadOnlyList<ResourceCapacity> ResourceCapacities => Data.BuildingLevels[CurrentLevel].ResourcesCapacity;
        public string Description => Data.Description;

        public int GetResourceCapacity(ResourceType resourceType)
        {
            var complexLevel = Data.BuildingLevels[CurrentLevel];
            return (from complexStorageData in complexLevel.ResourcesCapacity
                where resourceType == complexStorageData.ResourceType
                select complexStorageData.Capacity).FirstOrDefault();
        }

        public IStorageModel ImportStorage => _regionStorage;
        public IStorageModel ExportStorage => _regionStorage;
    }
}