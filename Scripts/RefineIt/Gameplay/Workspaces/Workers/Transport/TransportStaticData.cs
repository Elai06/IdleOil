using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Workspaces.Workers.Transport
{
    [CreateAssetMenu(fileName = "TransportData", menuName = "Data/Transport")]
    public class TransportStaticData : ScriptableObject
    {
        public TransportType Type;
        public AssetReferenceGameObject PrefabReference;
        public TransportLevelData[] Levels;
    }
}