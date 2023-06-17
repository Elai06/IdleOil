using System.Threading.Tasks;
using Gameplay.Currencies;
using Gameplay.Personnel;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Workers
{
    public class WorkersViewModel : ViewModelBase<WorkersService, WorkersView>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly CurrenciesModel _currenciesModel;
        private readonly IStaticDataService _staticDataService;

        public WorkersViewModel(WorkersService model, WorkersView view, IAssetProvider assetProvider,
            CurrenciesModel currenciesModel, IStaticDataService staticDataService) : base(model, view)
        {
            _assetProvider = assetProvider;
            _currenciesModel = currenciesModel;
            _staticDataService = staticDataService;
        }

        public override Task Show()
        {
            Initialize();

            return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            Model.Upgrade += UpdateWorkerSubView;
        }

        public override void Unsubscribe()
        {
            Model.Upgrade -= UpdateWorkerSubView;
            
            foreach (var subView in View.WorkerSubView.SubViews.Values)
            {
                subView.Click -= Model.UpgradeWorkerLevel;
            }
        }

        private new async void Initialize()
        {
            View.WorkerSubView.CleanUp();
            foreach (var personnelModel in Model.WorkerModels.Values)
            {
                var workersViewData = await CreateViewData(personnelModel);

                View.WorkerSubView.Add(personnelModel.WorkerData.WorkerType.ToString(), workersViewData);

                var subView = View.WorkerSubView.SubViews[workersViewData.Name];
                
                subView.Click += Model.UpgradeWorkerLevel;
            }
        }

        private async Task<WorkersViewData> CreateViewData(WorkerModel workerModel)
        {
            var progress = workerModel.WorkerProgress;
            var data = workerModel.WorkerData;
            var nextLevel = progress.Level + 1 <= data.WorkerLevelsData.Count - 1 ? progress.Level + 1 : progress.Level;

            WorkersViewData workersViewData = new()
            {
                WorkerImage = await _assetProvider.LoadSprite(data.WorkerSpriteReference),
                Name = data.WorkerType.ToString(),
                BonusValue = $"{data.WorkerLevelsData[progress.Level].Bonus}%",
                NextBonusValue = $"+{data.WorkerLevelsData[nextLevel].Bonus - data.WorkerLevelsData[progress.Level].Bonus}",
                Level = $"Ур.{progress.Level + 1}",
                Price = data.WorkerLevelsData[progress.Level].Price.ToString(),
                IsBuy = progress.IsBuy,
                IsMaxLevel = progress.Level == data.WorkerLevelsData.Count - 1,
                IsAvailable = _currenciesModel.Has(data.CurrencyType,
                    data.WorkerLevelsData[progress.Level].Price),

                ResourceImage = await _assetProvider.LoadSprite(_staticDataService
                    .GetResourceStaticData(data.ResourceType).SpriteAssetReference),

                PriceImage = await _assetProvider.LoadSprite(_staticDataService.GetCurrencyData(data.CurrencyType).Sprite)
            };

            return workersViewData;
        }
        
        private async void UpdateWorkerSubView(string id)
        {
            foreach (var model in Model.WorkerModels.Values)
            {
                var workersViewData = await CreateViewData(model);

                View.WorkerSubView.UpdateView(workersViewData, model.WorkerData.WorkerType.ToString());
            }
        }
    }
}