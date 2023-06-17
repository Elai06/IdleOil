using System.Linq;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Sates;

namespace Gameplay.Workspaces.Workers.States
{
    public class TransportIdleState : IState
    {
        private readonly TransportProgress _progress;

        private readonly IStorageModel _importStorage;
        private readonly IStorageModel _transportStorage;
        private IStateMachine _stateMachine;

        public TransportIdleState(TransportProgress progress, IStorageModel importStorage,
            IStorageModel transportStorage)
        {
            _progress = progress;
            _importStorage = importStorage;
            _transportStorage = transportStorage;
        }

        public bool CanEnterImportState => CheckCanTransportPlaceResources() && CheckCanTakeImportResources(); 
        
        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _progress.CurrentState = TransportState.Idle;

            if (CanEnterImportState)
            {
                _stateMachine.Enter<TransportImportState>();
            }
            else if(TransportStorageHasResources())
            {
                _stateMachine.Enter<ShippingState>();
            }
            else
            {
                _importStorage.ResourceChanged += OnResourceChanged;
            }
        }

        public void Exit()
        {
            _importStorage.ResourceChanged -= OnResourceChanged;
        }

        private bool TransportStorageHasResources()
        {
            return _transportStorage.Resources.Values.Any(resourceProgress => resourceProgress.Amount > 0);
        }

        private void OnResourceChanged(ResourceType resourceType, int amount)
        {
            if (CanEnterImportState)
                _stateMachine.Enter<TransportImportState>();
        }

        private bool CheckCanTransportPlaceResources()
        {
            return _importStorage.Resources.Values
                .Select(resource => _transportStorage.CanPlaceResources(resource.ResourceType))
                .Any(availableResources => availableResources != 0);
        }
        
        private bool CheckCanTakeImportResources()
        {
            return _importStorage.Resources.Values
                .Select(resource => _importStorage.CanTakeResources(resource.ResourceType))
                .Any(availableResources => availableResources != 0);
        }
    }
}