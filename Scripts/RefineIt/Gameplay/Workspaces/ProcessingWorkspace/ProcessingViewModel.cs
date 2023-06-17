using System.Linq;
using System.Threading.Tasks;
using Gameplay.Region.Storage;
using Gameplay.Services.Timer;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.StaticData.ProcessingWorkspace;
using Infrastructure.Windows.MVVM;
using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;
using Utils.Extensions;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public class ProcessingViewModel : ViewModelBase<ProcessingWorkspaceModel, ProcessingView>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly TimerService _timerService;

        private ProcessingType _currentType;

        public ProcessingViewModel(ProcessingWorkspaceModel model, ProcessingView view, IAssetProvider assetProvider,
            IStaticDataService staticDataService, TimerService timerService) : base(model, view)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
            _timerService = timerService;
        }

        public override async Task Show()
        {
            _currentType = Model.ProcessingType;

            View.ProduceSubViewContainer.CleanUp();
            View.RequiredSubViewContainer.CleanUp();
            View.RequiredConversionSubViewContainer.CleanUp();
            View.ProduceConversionSubViewContainer.CleanUp();

            await CreateResourceSubViewsData(View.ProduceSubViewContainer, Model.OutputResourcesStorage,
                Model.CurrentLevelData.ProduceStorageCapacity, Model.NextLevelData.ProduceStorageCapacity);
            await CreateResourceSubViewsData(View.RequiredSubViewContainer, Model.InputResourceStorage,
                Model.CurrentLevelData.RequiredStorageCapacity, Model.NextLevelData.RequiredStorageCapacity);

            CreateConversionResourceSubViews(View.RequiredConversionSubViewContainer,
                Model.CurrentLevelData.ResourceConversionData.InputResources);
            CreateConversionResourceSubViews(View.ProduceConversionSubViewContainer,
                Model.CurrentLevelData.ResourceConversionData.OutputResources);

            await ViewData();

            BuildedView();
        }

        public override void Subscribe()
        {
            Model.LevelUpdated += UpdateLevelView;
            Model.ProcessingTimer += TimeFill;
            Model.OutputResourcesStorage.ResourceChanged += OnUpdateOutputResourceSubViews;
            Model.InputResourceStorage.ResourceChanged += OnUpdateInputResourceSubViews;
        }
        
        public override void Unsubscribe()
        {
            Model.LevelUpdated -= UpdateLevelView;
            Model.ProcessingTimer -= TimeFill;
            Model.OutputResourcesStorage.ResourceChanged -= OnUpdateOutputResourceSubViews;
            Model.InputResourceStorage.ResourceChanged -= OnUpdateInputResourceSubViews;
        }

        private async Task ViewData()
        {
            Sprite processingSprite = await _assetProvider.LoadSprite(Model.LevelSprite);
            Sprite processingCurrencySprite =
                await _assetProvider.LoadSprite(_staticDataService.GetCurrencyData(Model.CostType).Sprite);
            View.ViewProcessingData(processingSprite, Model.CurrentLevelData.Description);
            int cost = Model.IsBuilded ? Model.CurrentLevelData.UpdateCost : Model.Cost;
            View.ViewProcessingCost(processingCurrencySprite, cost.ToFormattedBigNumber());
            View.TimeView(Model.CurrentLevelData.ProcessingTime);
            View.SetLevelView(Model.CurrentLevel);
            View.ViewButtons(Model.HasNextLevel || !Model.IsBuilded);
            TimeFill(0);
        }

        private async void UpdateLevelView(ILevelsBuilding levelsBuilding)
        {
            await UpdateProcessingSprite();
            await UpdateSubViewData(Model.OutputResourcesStorage, View.ProduceSubViewContainer,
                Model.CurrentLevelData.ProduceStorageCapacity, Model.NextLevelData.ProduceStorageCapacity);
            await UpdateSubViewData(Model.InputResourceStorage, View.RequiredSubViewContainer,
                Model.CurrentLevelData.RequiredStorageCapacity, Model.NextLevelData.RequiredStorageCapacity);
            await ViewData();

            View.ViewUpgrade(Model.HasNextLevel && Model.IsBuilded);
            View.PriceView(Model.HasNextLevel);
        }

        private async Task UpdateProcessingSprite()
        {
            View.UpdateProcessingSprite(await _assetProvider.LoadSprite(Model.LevelSprite));
        }
        
        private async void OnUpdateOutputResourceSubViews(ResourceType resourceType, int value)
        {
            await UpdateSubViewData(Model.OutputResourcesStorage, View.ProduceSubViewContainer,
                Model.CurrentLevelData.ProduceStorageCapacity, Model.NextLevelData.ProduceStorageCapacity);
            
        }
        
        private async void OnUpdateInputResourceSubViews(ResourceType resourceType, int value)
        {
            await UpdateSubViewData(Model.InputResourceStorage, View.RequiredSubViewContainer,
                Model.CurrentLevelData.RequiredStorageCapacity, Model.NextLevelData.RequiredStorageCapacity);
        }


        private async Task UpdateSubViewData(IStorageModel storage,
            SubViewContainer<ProcessingResourceSubView, ProcessingResourceData> subViewContainer,
            ResourceCapacity[] resourceCapacities, ResourceCapacity[] nextLevelResourceCapacities)
        {
            foreach (var resource in resourceCapacities)
            {
                var resourceAmount = storage.Resources.Count > 0 ? storage.Resources[resource.ResourceType].Amount : 0;
                var nextLevelResourceCapacity =
                    nextLevelResourceCapacities.First(x => x.ResourceType == resource.ResourceType);
                subViewContainer.UpdateView(
                    await CreateStorageSubViewData(resourceAmount, resource, nextLevelResourceCapacity),
                    resource.ResourceType.ToString());
            }
        }

        private async Task CreateResourceSubViewsData(
            SubViewContainer<ProcessingResourceSubView, ProcessingResourceData> subViewContainer, IStorageModel storage,
            ResourceCapacity[] resourceCapacities, ResourceCapacity[] nextLevelResourceCapacities)
        {
            foreach (var resource in resourceCapacities)
            {
                var resourceAmount = storage.Resources.Count > 0 ? storage.Resources[resource.ResourceType].Amount : 0;

                var nextLevelResourceCapacity =
                    nextLevelResourceCapacities.First(x => x.ResourceType == resource.ResourceType);

                subViewContainer.Add(resource.ResourceType.ToString(),
                    await CreateStorageSubViewData(resourceAmount, resource, nextLevelResourceCapacity));
            }
        }

        private async Task<ProcessingResourceData> CreateStorageSubViewData(int resourceAmount,
            ResourceCapacity resourceCapacity, ResourceCapacity nextLevelResourceCapacities)
        {
            Sprite sprite = await _assetProvider.LoadSprite(Model.GetResourceSprite(resourceCapacity.ResourceType));

            int resourceValue = (int)(resourceAmount * Model.GetResourceBonus(resourceCapacity.ResourceType));

            ProcessingResourceData data = new()
            {
                ResourceType = resourceCapacity.ResourceType,
                Value = resourceValue,
                ResourceSprite = sprite,
                ResourceCapacity = resourceCapacity.Capacity,
                NextLevelResourceCapacity = nextLevelResourceCapacities.Capacity - resourceCapacity.Capacity,
                IsBuilded = Model.IsBuilded
            };
            return data;
        }

        private async void CreateConversionResourceSubViews(
            SubViewContainer<IconDescriptionView, IconDescriptionData> viewRequiredConversionSubViewContainer,
            ResourceConversion[] inputResources)
        {
            foreach (var resource in inputResources)
            {
                var sprite = await _assetProvider.LoadSprite(_staticDataService
                    .GetResourceStaticData(resource.ResourceType).SpriteAssetReference);
                var data = new IconDescriptionData(sprite, resource.Value.ToString(),
                    resource.ResourceType.ToString());
                viewRequiredConversionSubViewContainer.Add(data.Id, data);
            }
        }

        private void BuildedView()
        {
            View.ViewBuilded(Model.IsBuilded);
            View.ViewUpgrade(Model.HasNextLevel && Model.IsBuilded);
            View.PriceView(Model.HasNextLevel || !Model.IsBuilded);
        }

        private void TimeFill(int time)
        {
            if (_currentType != Model.ProcessingType) return;

            var timeFill = time / (float)Model.CurrentLevelData.ProcessingTime;

            View.TimeFillView(timeFill);

            View.TimePause(time <= 0);
        }
    }
}