using System;
using Gameplay.Workspaces.MiningWorkspace;
using UnityEngine;

namespace Gameplay.Orders
{
    [Serializable]
    public class OrderResourceData
    {
        public ResourceType ResourceType;
        public Sprite ResourceSprite;
        public int Quantity;
    }
}