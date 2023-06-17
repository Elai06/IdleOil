using Gameplay.MVVM.Views;
using Gameplay.Orders;
using Gameplay.Services.Timer;
using Infrastructure.AssetManagement;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Order
{
    public class OrdersViewModelFactory : IViewModelFactory<OrdersViewModel, OrdersView, OrdersModel>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;

        public OrdersViewModelFactory(IStaticDataService staticDataService, IAssetProvider assetProvider,
            TimerService timerService)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
        }

        public OrdersViewModel Create(OrdersModel model, OrdersView view) =>
            new(model, view, _staticDataService, _assetProvider);
    }
}