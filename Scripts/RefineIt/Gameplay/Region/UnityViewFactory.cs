using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Region
{
    public class UnityViewFactory : IUnityViewFactory
    {
        private readonly IAssetProvider _assetProvider;
        
        public UnityViewFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public Task<GameObject> Create(AssetReferenceGameObject view,Vector3 spawnPos)
        {
            return _assetProvider.Instantiate(view,spawnPos);
        }
    }
}