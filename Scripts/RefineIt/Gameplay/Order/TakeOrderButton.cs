using Gameplay.Order;
using Gameplay.Region;

namespace Gameplay.Orders
{
    public class TakeOrderButton : ButtonBaseSFX
    {
        private OrderModel _orderModel;

        public void Initialize(OrderModel orderModel)
        {
            _orderModel = orderModel;
        }

        public override void OnClick()
        {
            base.OnClick();
            _orderModel.TakeOrder();
            gameObject.SetActive(false);
        }
    }
}