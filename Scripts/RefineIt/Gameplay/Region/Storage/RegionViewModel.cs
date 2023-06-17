using System.Threading.Tasks;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Infrastructure.Windows.MVVM.SubView;
using UnityEngine;

namespace Gameplay.Region.Storage
{
    public class RegionViewModel : ViewModelBase<IRegionStorage, RegionView>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public RegionViewModel(IRegionStorage model, RegionView view, IAssetProvider assetProvider,
            IStaticDataService staticDataService) : base(model, view)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public override async Task Show()
        {
            View.SubViewContainer.CleanUp();
            await ShowAll();
        }

        public override void Subscribe()
        {
            Model.ResourceChanged += OnResourceChanged;
            Model.CapacityChanges += OnResourceCapacityChange;
            Model.DictionaryResourceChanged += OnDictionaryResourceAdded;
        }

        public override void Unsubscribe()
        {
            Model.ResourceChanged -= OnResourceChanged;
            Model.CapacityChanges -= OnResourceCapacityChange;
            Model.DictionaryResourceChanged -= OnDictionaryResourceAdded;
        }

        private async void OnResourceCapacityChange(ResourceType resourceType, int capacity) =>
            await UpdateStorageResourcesSubViews(resourceType);

        private async void OnResourceChanged(ResourceType type, int amount) =>
            await UpdateStorageResourcesSubViews(type);

        private async Task ShowAll()
        {
            await CreateStorageResourcesSubViews();
        }

        private async Task CreateStorageResourcesSubViews()
        {
            foreach (var resource in Model.GetDictionaryResources().Values)
            {
                var sprite = await _assetProvider.LoadSprite(Model.GetResourceSprite(resource.ResourceType));
                var data = new IconDescriptionData(sprite,
                    $"{resource.Amount}/{Model.GetResourceCapacity(resource.ResourceType)}",
                    resource.ResourceType.ToString());
                View.SubViewContainer.Add(resource.ResourceType.ToString(), data);
            }
        }

        private async Task UpdateStorageResourcesSubViews(ResourceType type)
        {
            if (Model.GetDictionaryResources().TryGetValue(type, out var resource))
            {
                var sprite = await _assetProvider.LoadSprite(Model.GetResourceSprite(type));
                var data = new IconDescriptionData(sprite,
                    $"{resource.Amount}/{Model.GetResourceCapacity(resource.ResourceType)}", type.ToString());
                View.SubViewContainer.UpdateView(data, type.ToString());
            }
        }

        private async void OnDictionaryResourceAdded(ResourceType type)
        {
            if (Model.GetDictionaryResources().TryGetValue(type, out var resource))
            {
                var sprite = await _assetProvider.LoadSprite(Model.GetResourceSprite(type));
                var data = new IconDescriptionData(sprite,
                    $"{resource.Amount}/{Model.GetResourceCapacity(resource.ResourceType)}", type.ToString());
                View.SubViewContainer.Add(resource.ResourceType.ToString(), data);
            }
        }
    }
}