using System.Threading.Tasks;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.Workers.Path;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.AssetManagement;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Wagon
{
    public class TransportOrderFactory : ITransportOrderFactory
    {
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineService _coroutineService;
        private readonly IAssetProvider _assetProvider;

        public TransportOrderFactory(IProgressService progressService, IStaticDataService staticDataService,
            ICoroutineService coroutineService,
            IAssetProvider assetProvider)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
            _coroutineService = coroutineService;
            _assetProvider = assetProvider;
        }

        public async Task<TransportOrderModel> Create(TransportOrderPathMarker transportOrderPathMarker,
            OrderModel orderModel)
        {
            var data = _staticDataService.GetWagonData(transportOrderPathMarker.transportOrderType);

            var progress = _progressService.RegionProgress.GetWagon(transportOrderPathMarker.Guid);
            progress.WagonState = TransportState.Shipping;

            var wagonView = await _assetProvider.Load<GameObject>(data.PrefabReference);
            var wagonMover = Object.Instantiate(wagonView).GetComponent<TransportMover>();
            wagonMover.transform.position = transportOrderPathMarker.StartPoint.position;

            return new TransportOrderModel(data, progress, _coroutineService, wagonMover,
                transportOrderPathMarker.StartCurve, transportOrderPathMarker.EndCurve,
                orderModel, transportOrderPathMarker.EndPoint);
        }

        public void Update(TransportOrderPathMarker transportOrderPathMarker, OrderModel orderModel,
            TransportOrderModel transportOrderModels)
        {
            transportOrderModels.Mover.InitializePath(transportOrderPathMarker.StartCurve);
            transportOrderModels.OrderModel = orderModel;
            transportOrderModels.Progress.WagonState = TransportState.Shipping;
            transportOrderModels.Initialize();
        }
    }
}