using System.Linq;
using Gameplay.Currencies;
using Gameplay.Quests;
using Gameplay.Region.Storage;
using Gameplay.Tilemaps.Data;
using Gameplay.Workspaces.Buildings.LevelBuildings;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;

namespace Gameplay.Workspaces.CrudeOilStorage
{
    public class StorageOilCrudeModel :
        LevelBuildingBase<StorageOilCrudeProgress, StorageOilCrudeStaticData, StorageOilCrudeLevel>, IExportStorage
    {
        private readonly IRegionStorage _regionStorage;

        public StorageOilCrudeModel(StorageOilCrudeProgress progress, StorageOilCrudeStaticData data,
            CurrenciesModel currenciesModel, IRegionStorage regionStorage, IQuestModel questModel) : base(progress, data, currenciesModel, questModel)
        {
            _regionStorage = regionStorage;
        }

        public override string Id => BuildingType.Storage.ToString();
        public IStorageModel ExportStorage => _regionStorage;

        public int GetResourceCapacity(ResourceType resourceType)
        {
            var storageLevel = Data.BuildingLevels[CurrentLevel];

            return (from complexStorageData in storageLevel.ResourcesCapacity
                where resourceType == complexStorageData.ResourceType
                select complexStorageData.Capacity).FirstOrDefault();
        }
    }
}