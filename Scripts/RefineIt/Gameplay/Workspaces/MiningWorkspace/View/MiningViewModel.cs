using System.Threading.Tasks;
using Gameplay.Region.Storage;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using UnityEngine;
using Utils.Extensions;

namespace Gameplay.Workspaces.MiningWorkspace.View
{
    public class MiningViewModel : ViewModelBase<MiningWorkSpaceModel, MiningView>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private IRegionStorage _regionStorage;

        public MiningViewModel(MiningWorkSpaceModel model, MiningView view, IAssetProvider assetProvider,
            IStaticDataService staticDataService, IRegionStorage regionStorage) : base(model, view)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _regionStorage = regionStorage;
        }

        public override async Task Show()
        {
            var data = _staticDataService.MiningWorkspaceStaticData;
            var miningSprite = await _assetProvider.LoadSprite(Model.SpriteView);
            var miningCurrencySprite = await _assetProvider.LoadSprite
                (_staticDataService.GetCurrencyData(Model.CostType).Sprite);
            
            View.ViewData(miningSprite, Model.Description);
            View.ViewMiningCost(miningCurrencySprite, Model.Cost.ToFormattedBigNumber());
            View.ViewBuilded(Model.IsBuilded);
            
            var resourceValue = (int)(data.MinedResource * Model.GetResourceBonus(data.ResourceType));
            View.ViewResource(await _assetProvider.LoadSprite(_staticDataService
                .GetResourceStaticData(data.ResourceType).SpriteAssetReference), resourceValue, data.MiningDuration);
            
            UpdateView();
            SliderView();
        }

        public override void Subscribe()
        {
            base.Subscribe();
            _regionStorage.ResourceChanged += OnResourceChanged;
        }


        public override void Unsubscribe()
        {
            base.Unsubscribe();
            _regionStorage.ResourceChanged -= OnResourceChanged;
        }

        private void UpdateView()
        {
            UpdateSprite();
            View.ViewBuilded(Model.IsBuilded);
        }

        private void SliderView()
        {
            if (_regionStorage.GetDictionaryResources().TryGetValue(ResourceType.Oil, out var value))
            {
                View.SliderView(_regionStorage.GetResourceCapacity(ResourceType.Oil), value.Amount);
            }
        }

        private void OnResourceChanged(ResourceType resourceType, int value)
        {
            if (resourceType == ResourceType.Oil)
            {
                View.UpdateSliderValue(value);
            }
        }


        private async void UpdateSprite()
        {
            View.UpdateSprite(await _assetProvider.LoadSprite(Model.SpriteView));
        }
    }
}