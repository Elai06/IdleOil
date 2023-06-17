using System;
using System.Collections;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Sates;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Wagon
{
    public class TransportOrderShippingState : IState
    {
        private readonly ICoroutineService _coroutineService;
        private IStateMachine _stateMachine;
        private readonly TransportOrderProgress _progress;
        private TransportMover _mover;
        private readonly BezierCurve _curve;
        private readonly TransportOrderModel _model;

        public TransportOrderShippingState(ICoroutineService coroutineService, TransportMover mover, TransportOrderProgress progress,
            TransportOrderModel model, BezierCurve curve)
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
            _mover.InitializePath(_curve);
            _model.OrderModel.UpdateOrderStatus += OnUpdateOrderStatus;
            OnUpdateOrderStatus(_model.OrderModel);
        }

        public void Exit()
        {
            _model.OrderModel.UpdateOrderStatus -= OnUpdateOrderStatus;
        }

        private void OnUpdateOrderStatus(OrderModel orderModel)
        {
            if (orderModel.OrderProgress.OrderStatus == OrderStatus.Working ||
                orderModel.OrderProgress.OrderStatus == OrderStatus.AvailableReward)
            {
                _coroutineService.StartCoroutine(Move());
            }
        }

        private IEnumerator Move()
        {
            var currentTime = 0f;
            while (currentTime < _model.ShippingTime)
            {
                currentTime += Time.deltaTime;
                var lerp = Mathf.Lerp(0f, 0.99f, currentTime / _model.ReturnTime);
                _mover.Move(lerp);
                yield return 0;
            }
            
            _stateMachine.Enter<TransportOrderIdleState>();
        }
    }
}