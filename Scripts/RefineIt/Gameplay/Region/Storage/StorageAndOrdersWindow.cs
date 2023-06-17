using Gameplay.Orders;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Region.Storage
{
    public class StorageAndOrdersWindow : Window
    {
        [SerializeField] private OrdersViewInitializer ordersViewInitializer;
        [SerializeField] private StorageViewInitializer _storageViewInitializer;
        private OrdersModel _ordersModel;
        private IRegionStorage _regionStorage;

        [Inject]
        private void Construct(OrdersModel orderGenerationModel,IRegionStorage regionStorage)
        {
            _ordersModel = orderGenerationModel;
            _regionStorage = regionStorage;

        }

        private void OnEnable()
        {
            ordersViewInitializer.Initialize(_ordersModel);
            _storageViewInitializer.Initialize(_regionStorage);
        }
    }
}