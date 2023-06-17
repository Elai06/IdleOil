using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Workspaces.Workers.Wagon
{
    [CreateAssetMenu(fileName = "TransportOrderStaticData", menuName = "Data/Wagon")]
    public class TransportOrderStaticData : ScriptableObject
    {
        public TransportOrderType Type;
        public AssetReferenceGameObject PrefabReference;
    }
}