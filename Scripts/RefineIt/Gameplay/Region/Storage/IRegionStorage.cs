using System;
using System.Collections.Generic;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.StaticData;
using UnityEngine.AddressableAssets;

namespace Gameplay.Region.Storage
{
    public interface IRegionStorage : IStorageModel
    {
        void Initialize(StorageProgress regionProgress, IStaticDataService staticDataService);
        IReadOnlyDictionary<ResourceType, ResourceProgress> GetDictionaryResources();
        int GetResourceCapacity(ResourceType resourceResourceType);
        AssetReference GetResourceSprite(ResourceType type);
        event Action<ResourceType> DictionaryResourceChanged;
    }
}