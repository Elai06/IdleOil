using System;

namespace Infrastructure.StateMachine
{
    public interface IStateMachineWithPayload
    {
        void Enter(Type trigger, IPayload payload);
    }
}