using System;
using Gameplay.Order;
using Gameplay.Orders;

namespace Gameplay.MVVM.Views
{
    public static class OrderDescription
    {
        private static string _availableReward = "Доставлен!";
        private static string _working = "Заказ выполняется";
        private static string _idle = "Мне нужна доставка";

        public static string GetOrderDescription(OrderModel orderModel)
        {
            switch (orderModel.OrderProgress.OrderStatus)
            {
                case OrderStatus.AvailableReward:
                    return _availableReward;
                case OrderStatus.Working:
                    return _working;
                case OrderStatus.Idle:
                    return _idle;
                case OrderStatus.Complete:
                    break;
                case OrderStatus.Failed:
                    break;
                case OrderStatus.AdsReplace:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return "Error";
        }
    }
}