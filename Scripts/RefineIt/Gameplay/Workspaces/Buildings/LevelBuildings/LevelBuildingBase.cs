using System;
using Gameplay.Currencies;
using Gameplay.Quests;
using Gameplay.Tilemaps.Buildings;
using UnityEngine.AddressableAssets;

namespace Gameplay.Workspaces.Buildings.LevelBuildings
{
    public abstract class LevelBuildingBase<TLevelBuildingProgress, TBuildingData, TBuildingLevelData> :
        BuildingBase<TLevelBuildingProgress, TBuildingData>, ILevelsBuilding
        where TLevelBuildingProgress : LevelBuildingProgress
        where TBuildingData : LevelBuildingData<TBuildingLevelData>
        where TBuildingLevelData : BuildingLevelData
    {
        private readonly CurrenciesModel _currenciesModel;
        private readonly IQuestModel _questModel;

        protected LevelBuildingBase(TLevelBuildingProgress progress, TBuildingData data,
            CurrenciesModel currenciesModel, IQuestModel questModel) : base(progress, data, currenciesModel)
        {
            _currenciesModel = currenciesModel;
            _questModel = questModel;
        }

        public event Action<ILevelsBuilding> LevelUpdated;

        public AssetReferenceSprite LevelSprite => Data.BuildingLevels[CurrentLevel].SpriteView;
        public int CurrentLevel => Progress.CurrentLevel;
        public int NumbersBuildings => CurrentLevelData.Buildings;
        public int UpgradeUpdateCost => Data.BuildingLevels[CurrentLevel].UpdateCost;
        public CurrencyType UpgradeCostType => Data.BuildingLevels[CurrentLevel].CostType;
        public bool HasNextLevel => Progress.CurrentLevel < Data.BuildingLevels.Count - 1;

        public TBuildingLevelData CurrentLevelData => Data.BuildingLevels[CurrentLevel];

        public TBuildingLevelData NextLevelData => Data.BuildingLevels[Data.BuildingLevels.Count > CurrentLevel + 1 ? CurrentLevel + 1 : CurrentLevel];

        public bool CanUpdateLevel()
        {
            return Data.BuildingLevels.Count > CurrentLevel 
                   && _currenciesModel.Has(UpgradeCostType, UpgradeUpdateCost);
        }
        
        public virtual void UpdateLevel()
        {
            if (HasNextLevel)
            {
                _currenciesModel.Consume(UpgradeCostType, UpgradeUpdateCost);
                Progress.CurrentLevel++;
                LevelUpdated?.Invoke(this);
                _questModel.TaskDailyProgress(QuestsGuid.updateProizv, 1);
                _questModel.TaskWeeklyProgress(QuestsGuid.updateProizvWeek, 1);
            }
        }
    }
}