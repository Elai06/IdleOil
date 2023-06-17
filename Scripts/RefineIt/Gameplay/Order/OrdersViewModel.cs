using System.Threading.Tasks;
using Gameplay.MVVM.Views;
using Gameplay.Orders;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Utils.Extensions;

namespace Gameplay.Order
{
    public class OrdersViewModel : ViewModelBase<OrdersModel, OrdersView>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public OrdersViewModel(OrdersModel model, OrdersView view,
            IStaticDataService staticDataService, IAssetProvider assetProvider) : base(model, view)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public override Task Show()
        {
            CreateOrdersSubView();

            return Task.CompletedTask;
        }

        public override void Subscribe()
        {
            base.Subscribe();

            Model.UpdateOrderStatus += OnUpdateOrderStatus;
            Model.UpdateOrder += OnUpdateOrderStatus;
            Model.GenerateService.RemoveOrderInDictionary += OnRemoveOrderInDictionary;
            Model.TutorialGenerateService.RemoveOrderInDictionary += OnRemoveOrderInDictionary;
            Model.UpdateAdsButton += OnUpdateAdsButton;
        }

        private void OnUpdateAdsButton(bool isActive)
        {
            foreach (var orderSubView in View.OrderSubView.SubViews.Values)
            {
                orderSubView.SetAdsButton(isActive);
            }
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            Model.UpdateOrderStatus -= OnUpdateOrderStatus;
            Model.UpdateOrder -= OnUpdateOrderStatus;
            Model.GenerateService.RemoveOrderInDictionary -= OnRemoveOrderInDictionary;
            Model.TutorialGenerateService.RemoveOrderInDictionary -= OnRemoveOrderInDictionary;
            Model.UpdateAdsButton -= OnUpdateAdsButton;
        }

        private void OnRemoveOrderInDictionary(OrderModel orderModel)
        {
            orderModel.Tick -= SetTime;

            View.OrderSubView.Remove(orderModel.ID.ToString());
        }

        private void OnUpdateOrderStatus(OrderModel orderModel)
        {
            if (!View.OrderSubView.SubViews.ContainsKey(orderModel.ID.ToString()))
            {
                orderModel.Tick += SetTime;
            }

            View.OrderSubView.UpdateView(orderModel, orderModel.ID.ToString());
            SetTime(orderModel);
        }

        private void CreateOrdersSubView()
        {
            View.OrderSubView.CleanUp();

            foreach (var orderModel in Model.OrderModels.Values)
            {
                View.OrderSubView.Add(orderModel.ID.ToString(), orderModel);
                orderModel.Tick += SetTime;
                SetTime(orderModel);
            }
        }

        private void SetTime(OrderModel orderModel)
        {
            if (orderModel.IsTimer)
                if (View.OrderSubView.SubViews.ContainsKey(orderModel.ID.ToString()))
                    View.OrderSubView.SubViews[orderModel.ID.ToString()]
                        .SetTimer(FormatTime.HoursStringFormat(orderModel.Timer.TimeProgress.Time));
        }
    }
}