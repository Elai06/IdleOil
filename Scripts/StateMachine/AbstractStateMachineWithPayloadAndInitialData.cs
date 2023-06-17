using System;
using Stateless;
using Utils;
using Utils.ZenjectInstantiateUtil;

namespace Infrastructure.StateMachine
{
    public abstract class AbstractStateMachineWithPayloadAndInitialData<TPayload, TInitialData> : AbstractStateMachineWithPayload
    {
        public AbstractStateMachineWithPayloadAndInitialData(IInstantiateSpawner instantiateSpawner) : base(instantiateSpawner)
        {
        }
        
        public void Initialize(TPayload payload)
        {
            StateMachine = ConfigureStates();
            if (StateMachine == null)
            {
                this.LogError("State machine is null");
                return;
            }
        }

        protected abstract StateMachine<IPayloadState<TPayload>, Type> ConfigureStates(TPayload initialData);
    }
}