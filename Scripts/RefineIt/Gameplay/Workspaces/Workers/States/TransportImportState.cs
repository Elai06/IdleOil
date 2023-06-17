using System.Collections;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Sates;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.States
{
    public class TransportImportState : IState
    {
        private readonly TransportModel _model;
        private readonly ICoroutineService _coroutineService;
        private readonly TransportProgress _progress;

        private IStateMachine _stateMachine;
        private readonly IStorageModel _transportStorage;
        private readonly IStorageModel _importStorage;

        public TransportImportState(TransportModel model, ICoroutineService coroutineService, TransportProgress progress,
            IStorageModel transportStorage, IStorageModel importStorage)
        {
            _model = model;
            _coroutineService = coroutineService;
            _progress = progress;

            _transportStorage = transportStorage;
            _importStorage = importStorage;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _progress.CurrentState = TransportState.Import;
            _coroutineService.StartCoroutine(Import());
        }

        public void Exit()
        {
        }

        private IEnumerator Import()
        {
            yield return new WaitForSeconds(_model.ImportTime);

            foreach (var importResource in _transportStorage.Resources.Values)
            {
                var canTake = _importStorage.CanTakeResources(importResource.ResourceType);
                var canPlace = _transportStorage.CanPlaceResources(importResource.ResourceType);
                if (canTake >= canPlace)
                {
                    _importStorage.TakeResources(importResource.ResourceType, canPlace);
                    _transportStorage.AddResources(importResource.ResourceType, canPlace);
                }
                else
                {
                    _importStorage.TakeResources(importResource.ResourceType, canTake);
                    _transportStorage.AddResources(importResource.ResourceType, canTake);
                }
            }
            _stateMachine.Enter<ShippingState>();
        }
    }
}