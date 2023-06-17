using Gameplay.Region.Storage;
using Gameplay.Workspaces.Buildings.States;
using Gameplay.Workspaces.Pipes;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;

namespace Gameplay.Workspaces.Workers.Pipeline
{
    public class PipelineModel
    {
        private readonly StateMachine _stateMachine;

        public PipelineModel(IStorageModel importStorage, IStorageModel exportStorage, PipelineData pipelineData,
            ICoroutineService coroutineService, StateMachine stateMachine, PipelineStaticData transportStaticData)
        {
            _stateMachine = stateMachine;
            Initialize();
        }

        private void Initialize()
        {
            _stateMachine.Enter<IdleState>();
        }
    }
}