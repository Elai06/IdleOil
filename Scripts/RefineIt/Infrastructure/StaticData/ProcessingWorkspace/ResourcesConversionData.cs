using System;
using Gameplay.Workspaces.ComplexWorkspace;

namespace Infrastructure.StaticData.ProcessingWorkspace
{
    [Serializable]
    public class ResourcesConversionData
    {
        public ResourceConversion[] InputResources;
        public ResourceConversion[] OutputResources;
    }
}