using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Sates;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Wagon
{
    public class TransportOrderIdleState : IState
    {
        private IStateMachine _stateMachine;
        private TransportOrderProgress _transportOrderProgress;
        private readonly TransportOrderModel _transportOrderModel;
        private Transform _endPoint;
        
        public TransportOrderIdleState(TransportOrderProgress transportOrderProgress, TransportOrderModel transportOrderModel, Transform endPoint)
        {
            _transportOrderProgress = transportOrderProgress;
            _transportOrderModel = transportOrderModel;
            _endPoint = endPoint;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _transportOrderModel.OrderModel.UpdateOrderStatus += OnTrackOrderStatus;
            
            OnTrackOrderStatus(_transportOrderModel.OrderModel);
        }
        
        public void Exit()
        {
            _transportOrderModel.OrderModel.UpdateOrderStatus -= OnTrackOrderStatus;
        }

        private void OnTrackOrderStatus(OrderModel orderModel)
        {
            if (_transportOrderModel.OrderModel.OrderProgress.OrderStatus == OrderStatus.Complete)
            {
                _stateMachine.Enter<TransportOrderReturnState>();
            }
        }
    }
}