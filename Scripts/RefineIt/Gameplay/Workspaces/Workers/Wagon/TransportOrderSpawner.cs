using System.Collections.Generic;
using System.Linq;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StaticData;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Wagon
{
    public class TransportOrderSpawner : ITransportOrderSpawner
    {
        private readonly ITransportOrderFactory _transportOrderFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly OrderGenerateService _orderGenerateService;
        private readonly OrderTutorialGenerateService _orderTutorialGenerateService;

        private readonly Dictionary<string, TransportOrderPathMarker> _pathMarkers = new();

        private readonly Dictionary<string, TransportOrderModel> _transportOrderModels = new();

        public TransportOrderSpawner(IStaticDataService staticDataService,
            OrderGenerateService orderGenerateService, ITransportOrderFactory transportOrderFactory,
            OrderTutorialGenerateService orderTutorialGenerateService)
        {
            _staticDataService = staticDataService;
            _orderGenerateService = orderGenerateService;
            _transportOrderFactory = transportOrderFactory;
            _orderTutorialGenerateService = orderTutorialGenerateService;
        }

        public IEnumerable<TransportOrderModel> Wagons => _transportOrderModels.Values;

        public void Initialize()
        {
            InitializePathsMarkers();
            Subscribe();
        }

        private void InitializePathsMarkers()
        {
            var pathMarkers = Object.FindObjectsOfType<TransportOrderPathMarker>();
            foreach (var transportPath in pathMarkers)
                _pathMarkers[transportPath.Guid] = transportPath;
        }

        private void Subscribe()
        {
            _orderGenerateService.UpdateOrder += Spawn;
            _orderTutorialGenerateService.UpdateOrder += Spawn;
            _orderGenerateService.CreatedOrder += Initialization;
            _orderTutorialGenerateService.CreatedOrder += Initialization;
        }

        private async void Spawn(OrderModel orderModel)
        {
            if (orderModel.OrderProgress.OrderStatus != OrderStatus.Working)
            {
                return;
            }

            for (int i = 0; i < _pathMarkers.Values.Count; i++)
            {
                TransportOrderPathData transportOrderPathData =
                    _staticDataService.TransportOrderPathsStaticData.Paths[i];
                TransportOrderPathMarker transportOrderPathMarker = _pathMarkers[transportOrderPathData.Guid];

                if (_transportOrderModels.ContainsKey(transportOrderPathMarker.Guid))
                {
                    if (_transportOrderModels[transportOrderPathData.Guid].Progress.WagonState == TransportState.Return)
                    {
                        _transportOrderFactory.Update(transportOrderPathMarker, orderModel,
                            _transportOrderModels[transportOrderPathData.Guid]);

                        return;
                    }

                    continue;
                }

                TransportOrderModel transportOrderModel = await _transportOrderFactory.Create(
                    transportOrderPathMarker, orderModel);
                _transportOrderModels.Add(transportOrderPathMarker.Guid, transportOrderModel);
                return;
            }
        }

        public async void Initialization(OrderModel model)
        {
            _orderGenerateService.CreatedOrder -= Initialization;
            _orderTutorialGenerateService.CreatedOrder -= Initialization;

            for (int i = 0; i < _orderGenerateService.OrderModels.Count; i++)
            {
                var orderModel = _orderGenerateService.OrderModels.Values.ToList()[i];

                if (orderModel.OrderProgress.OrderStatus != OrderStatus.AvailableReward &&
                    orderModel.OrderProgress.OrderStatus != OrderStatus.Working)
                {
                    continue;
                }

                TransportOrderPathData transportOrderPathData =
                    _staticDataService.TransportOrderPathsStaticData.Paths[i];
                TransportOrderPathMarker transportOrderPathMarker = _pathMarkers[transportOrderPathData.Guid];

                if (_transportOrderModels.ContainsKey(transportOrderPathMarker.Guid))
                {
                    continue;
                }

                TransportOrderModel transportOrderModel = await _transportOrderFactory
                    .Create(transportOrderPathMarker, orderModel);

                _transportOrderModels.Add(transportOrderPathMarker.Guid, transportOrderModel);
            }
        }
    }
}