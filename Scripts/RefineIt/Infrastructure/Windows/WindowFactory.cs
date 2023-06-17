using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using MVVMLibrary.Enums;
using MVVMLibrary.Infrastructure;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;

namespace Infrastructure.Windows
{
    public class WindowFactory : IWindowFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;

        private readonly Dictionary<Layer, Canvas> _layers;
        private readonly IInstantiateSpawner _inject;

        public WindowFactory(IStaticDataService staticDataService, IAssetProvider assetProvider,
            LayersContainer layersContainer, IInstantiateSpawner inject)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _inject = inject;
            _layers = new Dictionary<Layer, Canvas>();
            foreach(var canvasLayerEntry in layersContainer.CanvasLayerEntry)
                _layers.Add(canvasLayerEntry.Layer, canvasLayerEntry.Canvas);
        }

        public async Task<Window> Create(WindowType windowType)
        {
            var windowStaticData = _staticDataService.GetWindowData(windowType);
            var parent = _layers[windowStaticData.Layer].transform;
            var prefab = await _assetProvider.Load<GameObject>(windowStaticData.Window);
            var windowInstance = _inject.Instantiate<Window>(prefab, parent);
            windowInstance.SetLayer(windowStaticData.Layer);
            return windowInstance;
        }
    }
}