using System.Collections.Generic;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Factories;
using Gameplay.Workspaces.Pipes;
using Gameplay.Workspaces.ProcessingWorkspace;
using Gameplay.Workspaces.Workers.Transport;
using Infrastructure.StaticData;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Pipeline
{
    public class PipelineSpawner : IPipelineSpawner
    {
        private readonly IPipelineFactory _pipelineFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly IBuildingService _buildingService;

        private readonly List<PipelineModel> _pipelineModels = new();

        public PipelineSpawner(IPipelineFactory pipelineFactory, IStaticDataService staticDataService,
            IBuildingService buildingService)
        {
            _pipelineFactory = pipelineFactory;
            _staticDataService = staticDataService;
            _buildingService = buildingService;
        }

        public void Initialize()
        {
            Subscribe();
            Spawn();
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

        public void Spawn()
        {
            foreach (var pipelineData in _staticDataService.PipelinesTilemapData.PipelinesData)
            {
                var importBuilding = _buildingService.GetBuilding(pipelineData.ImportBuildGuid);
                var exportBuilding = _buildingService.GetBuilding(pipelineData.ExportBuildGuid);

                if (importBuilding.IsBuilded && exportBuilding.IsBuilded)
                {
                    if (importBuilding is IExportStorage importStorage &&
                        exportBuilding is IImportStorage exportStorage)
                    {
                        var data = _staticDataService.GetPipelineStaticData(pipelineData.TransportType);

                        _pipelineModels.Add(_pipelineFactory.Create(importStorage.ExportStorage,
                            exportStorage.ImportStorage, pipelineData, data));
                    }
                }
            }
        }
    }
}