using Scripts.StateMachine;

namespace Infrastructure.StateMachine
{
    public interface IStateMachine
    {
        void Enter<TState>() where TState : class, IState;
    }
}