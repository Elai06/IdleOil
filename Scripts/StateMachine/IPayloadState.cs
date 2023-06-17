using System;

namespace Infrastructure.StateMachine
{
    public interface IPayloadState<in TPayload>
    {
        event Action Completed;
        void SetPayload<T>(T payload) where T : TPayload;
        void OnEnterState();
        void Tick();
        void OnExitState();
    }
}