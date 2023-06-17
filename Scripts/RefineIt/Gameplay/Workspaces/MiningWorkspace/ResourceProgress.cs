using System;

namespace Gameplay.Workspaces.MiningWorkspace
{
    [Serializable]
    public class ResourceProgress
    {
        public ResourceType ResourceType;
        public int Amount;

        public ResourceProgress(ResourceType resourceType, int amount = 0)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }
}