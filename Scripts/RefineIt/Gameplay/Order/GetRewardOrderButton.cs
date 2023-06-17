using Gameplay.Order;
using Gameplay.Region;
using Gameplay.Settings;

namespace Gameplay.Orders
{
    public class GetRewardOrderButton : ButtonBaseSFX
    {
        private OrderModel _orderModel;

        public void Initialize(OrderModel orderModel)
        {
            _orderModel = orderModel;
        }
        public override void OnClick()
        {
            base.OnClick();
            _orderModel.GetReward();
            gameObject.SetActive(false);
        }
    }
}