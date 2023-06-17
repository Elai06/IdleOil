using System;
using System.Collections.Generic;

namespace Gameplay.Workspaces.MiningWorkspace
{
    [Serializable]
    public class StorageProgress
    {
        public List<ResourceProgress> ResourcesProgress = new List<ResourceProgress>();
    }
}