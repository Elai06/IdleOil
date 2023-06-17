using System;
using Utils;

namespace Infrastructure.StateMachine
{
    public abstract class PayloadState<TConcrete> : IPayloadState<IPayload> where TConcrete : IPayload
    {
        protected TConcrete _payload;

        public event Action Completed;

        public void SetPayload<T>(T payload) where T : IPayload
        {
            if (payload is TConcrete data)
            {
                _payload = data;
            }
            else
            {
                this.LogError($"State {GetType()} has wrong payload type - {typeof(T)}, needed {typeof(TConcrete)}");
            }
        }

        public abstract void OnEnterState();

        public abstract void Tick();

        public abstract void OnExitState();

        protected void CompleteState()
        {
            Completed?.Invoke();
        }
    }
}