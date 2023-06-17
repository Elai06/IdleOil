using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Order
{
    [CreateAssetMenu(fileName = "ClientSpriteStorage", menuName = "Storage/ClientSpriteStorage", order = 0)]
    public class ClientSpriteStorage : ScriptableObject
    {
        public List<AssetReferenceSprite> ClientSprites;
    }
}