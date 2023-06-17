using System.Collections;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Sates;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.States
{
    public class ShippingState : IState
    {
        private readonly TransportProgress _progress;
        private readonly TransportMover _mover;
        private readonly TransportModel _model;
        private readonly ICoroutineService _coroutineService;
        private readonly BezierCurve _curve;

        private IStateMachine _stateMachine;

        public ShippingState(ICoroutineService coroutineService, TransportMover mover, TransportProgress progress,
            TransportModel model, BezierCurve curve)
        {
            _coroutineService = coroutineService;
            _mover = mover;
            _progress = progress;
            _model = model;
            _curve = curve;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _progress.CurrentState = TransportState.Shipping;
            _mover.InitializePath(_curve);
            _coroutineService.StartCoroutine(Move());
        }

        public void Exit()
        {
        }

        private IEnumerator Move()
        {
            var currentTime = 0f;
            while(currentTime < _model.ShippingTime)
            {
                currentTime += Time.deltaTime;
                var lerp = Mathf.Lerp(0f, 0.99f, currentTime / _model.ShippingTime);
                _mover.Move(lerp);
                yield return null;
            }

            _stateMachine.Enter<TransportExportState>();
        }
    }
}