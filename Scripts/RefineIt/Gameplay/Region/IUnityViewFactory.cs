using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Region
{
    public interface IUnityViewFactory
    {
        Task<GameObject> Create(AssetReferenceGameObject view,Vector3 spawnPos);
    }
}