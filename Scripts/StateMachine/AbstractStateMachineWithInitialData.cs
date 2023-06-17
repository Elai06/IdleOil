using System;
using Scripts.StateMachine;
using Utils;
using Stateless;
using Utils.ZenjectInstantiateUtil;

namespace Infrastructure.StateMachine
{
    public abstract class AbstractStateMachineWithInitialData<T> : AbstractStateMachine
    {
        protected AbstractStateMachineWithInitialData(IInstantiateSpawner instantiateSpawner) : base(instantiateSpawner)
        {
        }

        public void Initialize(T initialData)
        {
            StateMachine = ConfigureStates(initialData);
            if (StateMachine == null)
            {
                this.LogError("State machine is null");
                return;
            }
            StateMachine.State.Start();
        }

        protected abstract StateMachine<IState, Type> ConfigureStates(T initialData);
    }
}