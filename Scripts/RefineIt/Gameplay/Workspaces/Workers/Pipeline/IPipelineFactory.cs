using Gameplay.Region.Storage;
using Gameplay.Workspaces.Pipes;
using Gameplay.Workspaces.ProcessingWorkspace;
using Gameplay.Workspaces.Workers.Transport;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Pipeline
{
    public interface IPipelineFactory
    {
        PipelineModel Create(IStorageModel importStorage, IStorageModel exportStorage,
            PipelineData pipelinesData, PipelineStaticData transportStaticData);
    }
}