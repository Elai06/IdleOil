using System.Threading.Tasks;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.Workers.Path;
using Infrastructure.AssetManagement;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Workspaces.Workers.Transport
{
    public class TransportFactory : ITransportFactory
    {
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly ICoroutineService _coroutineService;
        private readonly IAssetProvider _assetProvider;

        public TransportFactory(IProgressService progressService, IStaticDataService staticDataService, ICoroutineService coroutineService,
            IAssetProvider assetProvider)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
            _coroutineService = coroutineService;
            _assetProvider = assetProvider;
        }

        public async Task<TransportModel> Create(TransportType transportType, IStorageModel importStorage, IStorageModel exportStorage,
            BezierCurve importCurve, BezierCurve exportCurve, string guid)
        {
            var progress = _progressService.RegionProgress.GetTransport(guid, transportType);
            var data = _staticDataService.GetTransportData(transportType);

            var transportView = await _assetProvider.Load<GameObject>(data.PrefabReference);
            var transport = Object.Instantiate(transportView).GetComponent<TransportMover>();
            transport.InitializePath(importCurve);
            return new TransportModel(data, progress, _coroutineService, transport,
                exportCurve,
                importCurve,
                importStorage,
                exportStorage);
        }
    }
}