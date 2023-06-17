using Gameplay.Currencies;
using Gameplay.Personnel;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Workers
{
    public class WorkersViewModelFactory : IViewModelFactory<WorkersViewModel, WorkersView, WorkersService>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly CurrenciesModel _currenciesModel;
        private readonly IStaticDataService _staticDataService;

        public WorkersViewModelFactory(IAssetProvider assetProvider, CurrenciesModel currenciesModel, IStaticDataService staticDataService)
        {
            _assetProvider = assetProvider;
            _currenciesModel = currenciesModel;
            _staticDataService = staticDataService;
        }

        public WorkersViewModel Create(WorkersService model, WorkersView view) => new(model, view, _assetProvider, _currenciesModel, _staticDataService);
    }
}