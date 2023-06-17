using System;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Wagon
{
    public class TransportOrderModel
    {
        private readonly StateMachine _stateMachine;

        private readonly TransportOrderStaticData _data;
        private readonly TransportOrderProgress _progress;

        public TransportOrderModel(TransportOrderStaticData data, TransportOrderProgress progress, ICoroutineService coroutineService,
            TransportMover mover, BezierCurve startCurve, BezierCurve returnPath, OrderModel orderModel,
            Transform endPoint)
        {
            Mover = mover;
            OrderModel = orderModel;
            _data = data;
            _progress = progress;
            
            _stateMachine = new StateMachine();
            _stateMachine.AddState(new TransportOrderReturnState(coroutineService, mover, _progress, this, returnPath));
            _stateMachine.AddState(new TransportOrderIdleState(_progress, this, endPoint));
            _stateMachine.AddState(new TransportOrderShippingState(coroutineService, mover, _progress, this, startCurve));
            
            Initialize();
        }

        public TransportMover Mover { get;}

        public OrderModel OrderModel { get; set; }

        public TransportOrderProgress Progress => _progress;

        public float ReturnTime = 2;
        public double ShippingTime = 2;
        
        public void Initialize()
        {
            switch (_progress.WagonState)
            {
                case TransportState.Idle:
                    _stateMachine.Enter<TransportOrderIdleState>();
                    break;
                case TransportState.Shipping:
                    _stateMachine.Enter<TransportOrderShippingState>();
                    break;
                case TransportState.Return:
                    _stateMachine.Enter<TransportOrderReturnState>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid Trandport State {_progress.WagonState}");
            }
        }
    }
}