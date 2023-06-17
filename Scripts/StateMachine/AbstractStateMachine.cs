using System;
using Scripts.StateMachine;
using Stateless;
using Utils;
using Utils.ZenjectInstantiateUtil;

namespace Infrastructure.StateMachine
{
    public abstract class AbstractStateMachine : IStateMachine
    {
        protected readonly IInstantiateSpawner InstantiateSpawner;
        
        protected StateMachine<IState, Type> StateMachine;

        protected AbstractStateMachine(IInstantiateSpawner instantiateSpawner)
        {
            InstantiateSpawner = instantiateSpawner;
        }

        public void Initialize()
        {
            StateMachine = ConfigureStates();
            if (StateMachine == null)
            {
                this.LogError("State machine is null");
                return;
            }
            StateMachine.State.Start();
        }

        public void Tick()
        {
            StateMachine?.State?.Tick();
        }

        public void Enter<TState>() where TState : class, IState
        {
            var type = typeof(TState);
            if (!StateMachine.CanFire(type))
            {
                this.LogError($"Can't change state to {type}");
                return;
            }
            
            StateMachine.State.End();
            StateMachine.Fire(type);
            StateMachine.State.Start();
        }

        protected abstract StateMachine<IState, Type> ConfigureStates();
    }
}