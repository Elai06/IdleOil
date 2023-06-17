using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Sates;

namespace Gameplay.Workspaces.Buildings.States
{
    public abstract class BuildingState : IState
    {
        protected IStateMachine StateMachine;
        
        public void Initialize(IStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Exit() { }
        public virtual void Enter() { }
        public virtual void OnClick() { }
    }
}