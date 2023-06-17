using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Quests.UI
{
    public class WeeklyQuestViewModelFactory : IViewModelFactory<WeeklyQuestViewModel, WeeklyQuestView, IQuestModel>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;

        public WeeklyQuestViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public WeeklyQuestViewModel Create(IQuestModel model, WeeklyQuestView view) => 
            new WeeklyQuestViewModel(model, view/*, _assetProvider, _staticDataService*/);
    }
}