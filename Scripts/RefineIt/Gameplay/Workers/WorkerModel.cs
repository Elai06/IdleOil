using Gameplay.Currencies;

namespace Gameplay.Workers
{
    public class WorkerModel
    {
        public WorkerProgress WorkerProgress { get; }
        public WorkerData WorkerData { get; }
        public WorkerLevelData CurrentLevelData => WorkerData.WorkerLevelsData[WorkerProgress.Level];

        private readonly CurrenciesModel _currenciesModel;

        public WorkerModel(WorkerData workerData, WorkerProgress workerProgress, CurrenciesModel currenciesModel)
        {
            WorkerData = workerData;
            WorkerProgress = workerProgress;
            _currenciesModel = currenciesModel;
        }

        public bool BuyOrUpgrade()
        {
            if (!_currenciesModel.Has(WorkerData.CurrencyType, CurrentLevelData.Price)
                || WorkerProgress.Level >= WorkerData.WorkerLevelsData.Count - 1)
            {
                return false;
            }
            
            _currenciesModel.Consume(WorkerData.CurrencyType, CurrentLevelData.Price);
            WorkerProgress.Level++;

            if (!WorkerProgress.IsBuy)
            {
                WorkerProgress.IsBuy = true;
            }

            return true;

        }
    }
}