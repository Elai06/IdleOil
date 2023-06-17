using System;
using System.Collections.Generic;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Factories;
using Gameplay.Workspaces.ProcessingWorkspace;
using Gameplay.Workspaces.Workers.Path;
using Infrastructure.StaticData;
using Object = UnityEngine.Object;

namespace Gameplay.Workspaces.Workers.Transport
{
    public class TransportSpawner : ITransportSpawner
    {
        private readonly ITransportFactory _transportFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IBuildingService _buildingService;

        private readonly Dictionary<string, TransportPathMarker> _pathMarkers = new();

        private readonly Dictionary<string, TransportModel> _transportModels = new();

        public TransportSpawner(ITransportFactory transportFactory, IStaticDataService staticDataService,
            IBuildingService buildingService)
        {
            _transportFactory = transportFactory;
            _staticDataService = staticDataService;
            _buildingService = buildingService;
        }

        public IEnumerable<TransportModel> Transports => _transportModels.Values;

        public void Initialize()
        {
            InitializePathsMarkers();
            Subscribe();
            Spawn();
        }

        private void InitializePathsMarkers()
        {
            var pathMarkers = Object.FindObjectsOfType<TransportPathMarker>();
            foreach (var transportPath in pathMarkers)
                _pathMarkers[transportPath.Guid] = transportPath;
        }

        private void Subscribe()
        {
            foreach (var building in _buildingService.Buildings)
                building.Builded += Spawn;
        }

        private void Unsubscribe()
        {
            foreach (var building in _buildingService.Buildings)
                building.Builded -= Spawn;
        }

        private async void Spawn(IBuilding building) => Spawn();

        private async void Spawn()
        {
            foreach (var transportPathData in _staticDataService.TransportPathsStaticData.Paths)
            {
                if (!_transportModels.ContainsKey(transportPathData.Guid))
                {
                    var importBuilding = _buildingService.GetBuilding(transportPathData.StartPoint);
                    var exportBuilding = _buildingService.GetBuilding(transportPathData.EndPoint);
                    if (importBuilding.IsBuilded && exportBuilding.IsBuilded)
                    {
                        if (importBuilding is IExportStorage exportStorage
                            && exportBuilding is IImportStorage importStorage)
                        {
                            var transportPathMarker = _pathMarkers[transportPathData.Guid];
                            var transportModel = await _transportFactory.Create(
                                transportPathData.TransportType, exportStorage.ExportStorage,
                                importStorage.ImportStorage, transportPathMarker.ImportCurve,
                                transportPathMarker.ExportCurve, transportPathData.Guid);

                            if (_transportModels.ContainsKey(transportPathData.Guid))
                            {
                                return;
                            }
                            
                            _transportModels.Add(transportPathData.Guid, transportModel);
                        }
                        else
                        {
                            throw new Exception($"Invalid transport path {importBuilding.Id} or {exportBuilding.Id}");
                        }
                    }
                }
            }
        }
    }
}