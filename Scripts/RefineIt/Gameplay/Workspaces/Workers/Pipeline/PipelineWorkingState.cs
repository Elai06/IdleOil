using System.Collections;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.Buildings.States;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Pipes;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Pipeline
{
    public class PipelineWorkingState : WorkingState
    {
        private readonly IStorageModel _exportStorage;
        private readonly IStorageModel _importStorage;
        private readonly ICoroutineService _coroutineService;
        private readonly PipelineData _pipelineData;
        private readonly PipelineStaticData _pipelineStaticData;

        public PipelineWorkingState(IStorageModel importStorage, IStorageModel exportStorage, PipelineData pipelineData,
            ICoroutineService coroutineService, PipelineStaticData pipelineStaticData)
        {
            _exportStorage = exportStorage;
            _importStorage = importStorage;
            _pipelineData = pipelineData;
            _coroutineService = coroutineService;
            _pipelineStaticData = pipelineStaticData;
        }

        public override void Enter()
        {
            _coroutineService.StartCoroutine(TransferResources());
        }

        private IEnumerator TransferResources()
        {
            TakeResources();
            yield return new WaitForSeconds(_pipelineData.Duration);
            PlaceResources();

            StateMachine.Enter<IdleState>();
        }

        private void TakeResources()
        {
            foreach (var resource in _pipelineStaticData.Resources)
            {
                _importStorage.TakeResources(resource, _pipelineStaticData.AmountResources);
            }
        }

        private void PlaceResources()
        {
            foreach (var resource in _pipelineStaticData.Resources)
            {
                _exportStorage.AddResources(resource, _pipelineStaticData.AmountResources);
            }
        }
    }
}