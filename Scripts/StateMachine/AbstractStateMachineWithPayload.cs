using System;
using System.Collections.Generic;
using Stateless;
using Utils;
using Utils.ZenjectInstantiateUtil;

namespace Infrastructure.StateMachine
{
    public abstract class AbstractStateMachineWithPayload : IStateMachineWithPayload
    {
        protected readonly IInstantiateSpawner InstantiateSpawner;

        protected StateMachine<IPayloadState<IPayload>, Type> StateMachine;
        protected Queue<TransitionParam> TransitionsQueue;

        protected AbstractStateMachineWithPayload(IInstantiateSpawner instantiateSpawner)
        {
            InstantiateSpawner = instantiateSpawner;
        }

        public void Initialize()
        {
            StateMachine = ConfigureStates();
        }

        public void Tick()
        {
            StateMachine?.State?.Tick();
        }

        public void Enter(Type trigger, IPayload payload)
        {
            if (!StateMachine.CanFire(trigger))
            {
                this.LogError($"Can't change state {StateMachine.State} to {trigger}");
                return;
            }

            StateMachine.State.OnExitState();
            StateMachine.Fire(trigger);
            StateMachine.State.SetPayload(payload);
            StateMachine.State.OnEnterState();
        }

        protected abstract StateMachine<IPayloadState<IPayload>, Type> ConfigureStates();

        public void SetTransitionsQueue(Queue<TransitionParam> queue)
        {
            TransitionsQueue = queue;
            ExecuteNextTransition();
        }

        protected void ExecuteNextTransition()
        {
            StateMachine.State.Completed -= ExecuteNextTransition;

            if (TransitionsQueue.Count == 0)
            {
                return;
            }

            try
            {
                var nextState = TransitionsQueue.Peek();
                Enter(nextState.Type, nextState.Payload);
                TransitionsQueue.Dequeue();
            }
            catch (Exception e)
            {
                this.LogError($"{e}");
            }

            StateMachine.State.Completed += ExecuteNextTransition;
        }
    }
}