using System;
using System.Collections.Generic;
using Gameplay.Workspaces.MiningWorkspace;

namespace Gameplay.Region
{
    [Serializable]
    public class StorageProgress
    {
        public List<ResourceProgress> ResourceProgress;
        
        public StorageProgress()
        {
            ResourceProgress = new List<ResourceProgress>();
        }

        public ResourceProgress GetOrCreate(ResourceType type)
        {
            foreach (var value in ResourceProgress)
            {
                if (value.ResourceType == type)
                {
                    return value;
                }
            }

            var resourceProgress = new ResourceProgress(type,0);
            ResourceProgress.Add(resourceProgress);
            return resourceProgress;
        }
             
    }
}