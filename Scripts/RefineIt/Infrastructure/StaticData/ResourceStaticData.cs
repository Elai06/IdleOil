using Gameplay.Workspaces.MiningWorkspace;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "Resource_name", menuName = "Data/ResourceData")]
    public class ResourceStaticData : ScriptableObject
    {
        public ResourceType ResourceType;
        public AssetReferenceSprite SpriteAssetReference;
    }
}