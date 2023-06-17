namespace Gameplay.Workspaces.Buildings.States
{
    public class ComplexIdleState : IdleState
    {
        public override void Enter()
        {
            StateMachine.Enter<WorkingState>();
        }
    }
}