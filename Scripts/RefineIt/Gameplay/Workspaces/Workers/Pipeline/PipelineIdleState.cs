using System.Linq;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.Buildings.States;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Pipes;
using Gameplay.Workspaces.Workers.Transport;

namespace Gameplay.Workspaces.Workers.Pipeline
{
    public class PipelineIdleState : IdleState
    {
        private readonly IStorageModel _importStorage;
        private readonly IStorageModel _exportStorage;
        private PipelineStaticData _pipelineStaticData;

        public PipelineIdleState(IStorageModel exportStorage, IStorageModel importStorage,
            PipelineStaticData pipelineStaticData)
        {
            _importStorage = importStorage;
            _exportStorage = exportStorage;
            _pipelineStaticData = pipelineStaticData;
        }

        public override void Exit()
        {
            _importStorage.ResourceChanged -= OnChangeResourcesStorage;
            _exportStorage.ResourceChanged -= OnChangeResourcesStorage;
        }

        public override void Enter()
        {
            _importStorage.ResourceChanged += OnChangeResourcesStorage;
            _exportStorage.ResourceChanged += OnChangeResourcesStorage;

            OnChangeResourcesStorage();
        }

        private void OnChangeResourcesStorage(ResourceType arg1 = ResourceType.Bitumen, int arg2 = 0)
        {
            if (CheckCanPlaceResourcesFromExportStorage() && CheckCanTakeResourcesFromExportStorage())
            {
                StateMachine.Enter<WorkingState>();
            }
        }

        private bool CheckCanTakeResourcesFromExportStorage()
        {
            return _pipelineStaticData.Resources
                .Any(x => _importStorage.CanTakeResources(x, _pipelineStaticData.AmountResources));
        }

        private bool CheckCanPlaceResourcesFromExportStorage()
        {
            return _pipelineStaticData.Resources
                .Any(x => _exportStorage.CanPlaceResources(x) >= _pipelineStaticData.AmountResources);
        }
    }
}