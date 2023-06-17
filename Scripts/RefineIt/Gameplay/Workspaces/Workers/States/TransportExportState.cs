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
    public class TransportExportState : IState
    {
        private readonly TransportProgress _progress;
        private readonly TransportModel _model;
        private readonly ICoroutineService _coroutineService;

        private readonly IStorageModel _transportStorage;
        private readonly IStorageModel _exportStorage;
        
        private IStateMachine _stateMachine;

        public TransportExportState(TransportProgress progress, TransportModel model, IStorageModel transportStorage,
            IStorageModel exportStorage,
            ICoroutineService coroutineService)
        {
            _progress = progress;
            _model = model;
            _transportStorage = transportStorage;
            _exportStorage = exportStorage;
            _coroutineService = coroutineService;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _progress.CurrentState = TransportState.Export;
            _coroutineService.StartCoroutine(Export());
        }

        public void Exit()
        {
        }

        private IEnumerator Export()
        {
            yield return new WaitForSeconds(_model.ExportTime);

            foreach (var exportResource in _transportStorage.Resources.Values)
            {
                var canTake = _transportStorage.CanTakeResources(exportResource.ResourceType);
                var canPlace = _exportStorage.CanPlaceResources(exportResource.ResourceType);

                if (canPlace >= canTake)
                {
                    _exportStorage.AddResources(exportResource.ResourceType, canTake);
                    _transportStorage.TakeResources(exportResource.ResourceType, canTake);
                }
                else
                {
                    if (canPlace > 0)
                    {
                        _exportStorage.AddResources(exportResource.ResourceType, canPlace);
                        _transportStorage.TakeResources(exportResource.ResourceType, canPlace);
                    }
                }
            }

            _stateMachine.Enter<ReturnState>();
        }
    }
}