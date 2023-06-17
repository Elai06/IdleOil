using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Order;
using Gameplay.Region.Storage;
using Gameplay.Services.Timer;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Zenject;

namespace Gameplay.Orders
{
    public class OrdersModel
    {
        public event Action<OrderModel> UpdateOrderStatus;
        public event Action<OrderModel> UpdateOrder;
        public event Action<bool> UpdateAdsButton;


        private OrderGenerateService _orderGenerateService;
        private OrderTutorialGenerateService _orderTutorialGenerateService;
        private IRegionStorage _regionStorage;
        private IProgressService _progressService;
        private TimerService _timerService;

        [Inject]
        public void Construct(IStaticDataService staticDataService, OrderGenerateService orderGenerateService,
            IRegionStorage regionStorage, IProgressService progressService, TimerService timerService,OrderTutorialGenerateService orderTutorialGenerateService)
        {
            _regionStorage = regionStorage;
            _orderGenerateService = orderGenerateService;
            _progressService = progressService;
            _timerService = timerService;
            _orderTutorialGenerateService = orderTutorialGenerateService;

            Subscribe();
        }

        public Dictionary<int, OrderModel> OrderModels => _orderGenerateService.OrderModels;
        public OrderGenerateService GenerateService => _orderGenerateService;
        public OrderTutorialGenerateService TutorialGenerateService => _orderTutorialGenerateService;
        
        private void Subscribe()
        {
            _regionStorage.ResourceChanged += OnUpdateOrderStatus;
            _orderGenerateService.CreatedOrder += OnUpdateOrder;
            _orderGenerateService.UpdateOrder += OnUpdateOrder;
            _orderTutorialGenerateService.CreatedOrder += OnUpdateOrder;
            _orderTutorialGenerateService.UpdateOrder += OnUpdateOrder;
            _orderGenerateService.AdsStatus.UpdateAdsButtonStatus += OnAdsButtonUpdate;
        }

        private void OnAdsButtonUpdate(bool isActive)
        {
            UpdateAdsButton?.Invoke(isActive);
        }

        private void OnUpdateOrder(OrderModel model)
        {
            UpdateOrder?.Invoke(model);

            OnUpdateOrderStatus();
        }

        private void OnUpdateOrderStatus(ResourceType arg1 = ResourceType.Bitumen, int arg2 = 0)
        {
            foreach (var order in _orderGenerateService.OrderModels.Values
                         .Where(order => order.OrderProgress.OrderStatus == OrderStatus.Working))
            {
                if (order.ChangeOrderStatus(CheckOrderStatus(order)))
                {
                    UpdateOrderStatus?.Invoke(order);
                }
            }
        }

        private OrderStatus CheckOrderStatus(OrderModel orderModel)
        {
            return orderModel.ResourcesData.Any(resource =>
                !_regionStorage.CanTakeResources(resource.ResourceType, resource.Quantity))
                ? OrderStatus.Working
                : OrderStatus.AvailableReward;
        }
    }
}