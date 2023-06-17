using Gameplay.Region.Storage;
using Gameplay.Workspaces.Buildings.States;
using Gameplay.Workspaces.Pipes;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;

namespace Gameplay.Workspaces.Workers.Pipeline
{
    public class PipelineFactory : IPipelineFactory
    {
        private readonly ICoroutineService _coroutine;

        public PipelineFactory(ICoroutineService coroutine)
        {
            _coroutine = coroutine;
        }

        public PipelineModel Create(IStorageModel importStorage, IStorageModel exportStorage,
            PipelineData pipelinesData, PipelineStaticData transportStaticData)
        {
            StateMachine stateMachine = new();

            IdleState idleState = new PipelineIdleState(exportStorage, importStorage, transportStaticData);
            WorkingState workingState = new PipelineWorkingState(importStorage, exportStorage, pipelinesData,
                _coroutine, transportStaticData);

            stateMachine.AddState(idleState);
            stateMachine.AddState(workingState);

            return new PipelineModel(importStorage, exportStorage, pipelinesData, _coroutine, stateMachine, transportStaticData);
        }
    }
}