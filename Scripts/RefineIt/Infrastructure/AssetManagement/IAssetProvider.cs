using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        void Initialize();
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        Task<GameObject> Instantiate(string address, Vector3 at);
        Task<GameObject> Instantiate(string address);
        void Cleanup();

        Task<GameObject> Instantiate(AssetReference address, Transform parent);
        Task<GameObject> Instantiate(AssetReference address, Vector3 at);
        Task<GameObject> Instantiate(AssetReference address);
        Task<Sprite> LoadSprite(AssetReference assetReference);
    }
}