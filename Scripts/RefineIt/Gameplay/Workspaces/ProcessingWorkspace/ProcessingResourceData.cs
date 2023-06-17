using Gameplay.Workspaces.MiningWorkspace;
using UnityEngine;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    public class ProcessingResourceData
    {
        public ResourceType ResourceType;
        public Sprite ResourceSprite;
        public int Value;
        public int ResourceCapacity;
        public int NextLevelResourceCapacity;
        public bool IsBuilded;
    }
}