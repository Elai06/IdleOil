using System.Threading.Tasks;
using Gameplay.Region.Storage;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Workspaces.ComplexWorkspace
{
    public class ComplexViewModel : ViewModelBase<ComplexWorkspaceModel, ComplexView>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IRegionStorage _regionStorage;

        public ComplexViewModel(ComplexWorkspaceModel model, ComplexView view, IAssetProvider assetProvider,
            IStaticDataService staticDataService, IRegionStorage regionStorage) : base(model, view)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _regionStorage = regionStorage;
        }

        public override async Task Show()
        {
            var complexSprite = await _assetProvider.LoadSprite(Model.LevelSprite);
            var complexCurrencySprite =
                await _assetProvider.LoadSprite(_staticDataService.GetCurrencyData(Model.CostType).Sprite);
            View.ViewComplexData(complexSprite, Model.Description);
            var cost = Model.IsBuilded ? Model.CurrentLevelData.UpdateCost : Model.Cost;
            View.ViewComplexCost(complexCurrencySprite, cost.ToString());
            View.ViewUpgrade(Model.HasNextLevel && Model.IsBuilded);
            CreateSubView();
            BuildedView();
        }

        public override void Subscribe()
        {
            base.Subscribe();
            Model.LevelUpdated += UpdateLevelView;
            _regionStorage.ResourceChanged += OnResourceChanged;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            Model.LevelUpdated -= UpdateLevelView;
            _regionStorage.ResourceChanged -= OnResourceChanged;
        }

        private void OnResourceChanged(ResourceType resourceType, int value)
        {
            if (View.ResourceSubViews.SubViews.ContainsKey(resourceType.ToString()))
            {
                View.ResourceSubViews.SubViews[resourceType.ToString()].UpdateSliderValue(value);
            }
        }

        private void BuildedView()
        {
            View.ViewBuilded(Model.IsBuilded);
            View.ViewUpgrade(Model.HasNextLevel && Model.IsBuilded);
            View.ViewPrice(Model.HasNextLevel || !Model.IsBuilded);
        }

        private void UpdateLevelView(ILevelsBuilding levelsBuilding)
        {
            UpdateComplexSprite();
            UpdateResourceCapacityViews();

            var cost = Model.IsBuilded ? Model.CurrentLevelData.UpdateCost : Model.Cost;
            View.UpdateCost(cost.ToString());
            View.ViewUpgrade(Model.HasNextLevel && Model.IsBuilded);
            View.ViewPrice(Model.HasNextLevel);
        }

        private async void UpdateComplexSprite()
        {
            var loadSprite = await _assetProvider.LoadSprite(Model.LevelSprite);
            View.UpdateComplexSprite(loadSprite);
        }

        private async void CreateSubView()
        {
            View.ResourceSubViews.CleanUp();
            
            for (var i = 0; i < Model.ResourceCapacities.Count; i++)
            {
                var resourceCapacity = Model.ResourceCapacities[i];
                var spriteReference = _staticDataService
                    .GetResourceStaticData(resourceCapacity.ResourceType).SpriteAssetReference;
                var resourceSprite = await _assetProvider.LoadSprite(spriteReference);

                var resourceValue = 0;

                if (_regionStorage.Resources.ContainsKey(resourceCapacity.ResourceType))
                {
                    resourceValue = _regionStorage.Resources[resourceCapacity.ResourceType].Amount;
                }

                ComplexResourceData complexResource = new()
                {
                    ResourceType = resourceCapacity.ResourceType,
                    ResourceSprite = resourceSprite,
                    ResourceCapacity = resourceCapacity.Capacity,
                    Value = resourceValue
                };

                View.InitializeSubView(complexResource);
            }
        }

        private void UpdateResourceCapacityViews()
        {
            for (var i = 0; i < Model.ResourceCapacities.Count; i++)
            {
                var resourceCapacity = Model.ResourceCapacities[i];
                var resourceValue = 0;

                if (_regionStorage.Resources.ContainsKey(resourceCapacity.ResourceType))
                {
                    resourceValue = _regionStorage.Resources[resourceCapacity.ResourceType].Amount;
                }
                
                View.ResourceSubViews.SubViews[resourceCapacity.ResourceType.ToString()]
                    .ViewUpdateCapacity(resourceValue, resourceCapacity.Capacity);
            }
        }
    }
}