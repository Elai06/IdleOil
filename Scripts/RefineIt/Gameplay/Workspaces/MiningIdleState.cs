using Gameplay.Workspaces.Buildings.States;

namespace Gameplay.Workspaces
{
    public class MiningIdleState : IdleState
    {
        public override void Enter()
        {
            StateMachine.Enter<WorkingState>();
        }
    }
}