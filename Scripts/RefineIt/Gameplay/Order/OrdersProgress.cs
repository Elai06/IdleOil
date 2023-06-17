using System;
using System.Collections.Generic;
using Gameplay.Orders;
using Utils.Extensions;

namespace Gameplay.Order
{
    [Serializable]
    public class OrdersProgress
    {
        public List<OrderProgress> OrderProgresses;

        public OrdersProgress()
        {
            OrderProgresses = new List<OrderProgress>();
        }

        public OrderProgress GetOrCreate(int ID, OrderRewardData orderRewardData = null,
            List<OrderResourceData> orderResourceDatas = null, int time = 0, bool isCanShowAds = true)
        {
            foreach (var order in OrderProgresses)
            {
                if (order.ID == ID)
                {
                    return order;
                }
            }

            OrderProgress orderProgress = new(OrderStatus.Idle, ID, orderRewardData,
                orderResourceDatas)
            {
                isCanShowAds = isCanShowAds,
                Time = FormatTime.HoursIntFormat(time),
                ClientSpriteIndex = -1
            };

            OrderProgresses.Add(orderProgress);
            return orderProgress;
        }
    }
}