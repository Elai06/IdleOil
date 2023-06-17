using Gameplay.Region;

namespace Gameplay.Order
{
    public class ReplaceOrderButton : ButtonBaseSFX
    {
        private OrderModel _orderModel;


        public void Initialize(OrderModel orderModel)
        {
            _orderModel = orderModel;
        }

        public override void OnClick()
        {
            base.OnClick();
            _orderModel.ShowAds();
        }
    }
}