using Gameplay.Region.Storage;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using Infrastructure.Windows.MVVM;
using Utils.ZenjectInstantiateUtil;

namespace Gameplay.Quests.UI
{
    public class DailyQuestViewModelFactory : IViewModelFactory<DailyQuestViewModel, DailyQuestView, IQuestModel>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;

        public DailyQuestViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider, IWindowFactory windowFactory, IInstantiateSpawner instantiateSpawner)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public DailyQuestViewModel Create(IQuestModel model, DailyQuestView view) => 
            new DailyQuestViewModel(model, view);
    }
}