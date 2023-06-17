using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Utils.ZenjectInstantiateUtil;

namespace Gameplay.DailyEntry.UI
{
    public class DailyEntryViewModelFactory : IViewModelFactory<DailyEntruViewModel, DailyEntryView, IDailyEntryModel>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;

        public DailyEntryViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider, IInstantiateSpawner instantiateSpawner)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public DailyEntruViewModel Create(IDailyEntryModel model, DailyEntryView view) => 
            new DailyEntruViewModel(model, view);
    }
}