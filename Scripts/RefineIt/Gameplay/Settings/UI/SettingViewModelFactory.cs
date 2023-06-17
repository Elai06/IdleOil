using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Settings.UI
{
    public class SettingViewModelFactory: IViewModelFactory<SettingViewModel, SettingView, ISettingModel>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;

        public SettingViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public SettingViewModel Create(ISettingModel model, SettingView view) => 
            new SettingViewModel(model, view);
    }
}