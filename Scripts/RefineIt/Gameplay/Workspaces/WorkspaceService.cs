using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.CrudeOilStorage;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData.ProcessingWorkspace;
using UnityEngine;

namespace Gameplay.Workspaces
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly Dictionary<Vector3Int, MiningWorkSpaceModel> _miningWorkspaces = new();
        private readonly Dictionary<Vector3Int, ProcessingWorkspaceModel> _processingWorkspaces = new();
        private readonly Dictionary<Vector3Int, ComplexWorkspaceModel> _complexWorkspaceModels = new();
        private readonly Dictionary<Vector3Int, StorageOilCrudeModel> _storageOilCrudeWorkspaceModels = new();

        private readonly IProgressService _progressService;
        private readonly IWorkspaceFactory _workspaceFactory;

        public WorkspaceService(IProgressService progressService, IWorkspaceFactory workspaceFactory)
        {
            _progressService = progressService;
            _workspaceFactory = workspaceFactory;
        }

        public event Action<ResourceType, int> CapacityChanged;

        public Dictionary<Vector3Int, MiningWorkSpaceModel> MiningWorkspaces => _miningWorkspaces;
        public Dictionary<Vector3Int, ProcessingWorkspaceModel> ProcessingWorkspaces => _processingWorkspaces;
        public Dictionary<Vector3Int, ComplexWorkspaceModel> ComplexWorkspaceModels => _complexWorkspaceModels;

        public void InitializeModels()
        {
            InitializeProcessingWorkspaces();
            InitializeMiningWorkspaces();
            InitializeOilStorageWorkspaces();
            InitializeComplexWorkspaces();
        }

        public void Cleanup()
        {
            UnsubscribeComplexies();
        }

        private void UnsubscribeComplexies()
        {
            foreach(var complexModel in _complexWorkspaceModels.Values)
            {
                complexModel.LevelUpdated -= OnComplexUpdateLevel;
                complexModel.Builded -= OnComplexBuilded;
            }
        }

        public ProcessingWorkspaceModel GetProcessModel(Vector3Int guid, ProcessingType processingType)
        {
            if(_processingWorkspaces.TryGetValue(guid, out var processingWorkspace))
                return processingWorkspace;

            var processingWorkspaceModel = _workspaceFactory.CreateProcessingWorkspaceModel(processingType, guid);
            _processingWorkspaces[guid] = processingWorkspaceModel;
            return processingWorkspaceModel;
        }

        public ComplexWorkspaceModel GetComplexModel(Vector3Int guid, ComplexType complexType)
        {
            if(_complexWorkspaceModels.TryGetValue(guid, out var complexWorkspace))
                return complexWorkspace;

            var complexWorkspaceModel = _workspaceFactory.CreateComplexWorkspaceModel(guid, complexType);
            _complexWorkspaceModels[guid] = complexWorkspaceModel;
            complexWorkspaceModel.LevelUpdated += OnComplexUpdateLevel;
            complexWorkspaceModel.Builded += OnComplexBuilded;
            return complexWorkspaceModel;
        }


        public MiningWorkSpaceModel GetMiningModel(Vector3Int guid)
        {
            if(_miningWorkspaces.TryGetValue(guid, out var miningWorkSpace))
                return miningWorkSpace;

            var miningWorkSpaceModel = _workspaceFactory.CreateMiningWorkSpaceModel(guid);
            _miningWorkspaces[guid] = miningWorkSpaceModel;
            return miningWorkSpaceModel;
        }

        public StorageOilCrudeModel GetOilCrudeModel(Vector3Int guid)
        {
            if(_storageOilCrudeWorkspaceModels.TryGetValue(guid, out var storageOilCrudeWorkspace))
                return storageOilCrudeWorkspace;

            var storageOilCrudeModel =
                _workspaceFactory.CreateStorageOilCrudeWorkspaceModel(guid);
            _storageOilCrudeWorkspaceModels[guid] = storageOilCrudeModel;
            return storageOilCrudeModel;
        }

        public int GetComplexResourceCapacity(ResourceType resourceType) =>
            _complexWorkspaceModels.Values.Where(value => value.IsBuilded).Sum(complexWorkspaceModel =>
                complexWorkspaceModel.GetResourceCapacity(resourceType));

        public int GetStorageOilCrudeCapacity(ResourceType resourceType) =>
            _storageOilCrudeWorkspaceModels.Values.Where(value => value.IsBuilded)
                .Sum(value => value.GetResourceCapacity(resourceType));

        public int GetResourceCapacity(ResourceType resourceType) =>
            GetComplexResourceCapacity(resourceType) + GetStorageOilCrudeCapacity(resourceType);

        private void InitializeProcessingWorkspaces()
        {
            foreach(var processingWorkSpace in _progressService.RegionProgress
                        .ProcessingWorkspaceProgresses)
            {
                var processingWorkspaceModel =
                    _workspaceFactory.CreateProcessingWorkspaceModel(processingWorkSpace.Id, processingWorkSpace.Guid);
                _processingWorkspaces[processingWorkspaceModel.Guid] = processingWorkspaceModel;
            }
        }

        private void InitializeMiningWorkspaces()
        {
            foreach(var miningWorkspaceProgress in _progressService.RegionProgress.MiningWorkSpaceProgresses)
            {
                var miningWorkSpaceModel = _workspaceFactory.CreateMiningWorkSpaceModel(miningWorkspaceProgress.Guid);
                _miningWorkspaces[miningWorkSpaceModel.Guid] = miningWorkSpaceModel;
            }
        }

        private void InitializeOilStorageWorkspaces()
        {
            foreach(var oilCrudeWorkspaceProgress in _progressService.RegionProgress.StorageOilCrudeWorkspaceProgresses)
            {
                var storageOilCrude =
                    _workspaceFactory.CreateStorageOilCrudeWorkspaceModel(oilCrudeWorkspaceProgress.Guid);
                _storageOilCrudeWorkspaceModels[storageOilCrude.Guid] = storageOilCrude;
            }
        }

        private void InitializeComplexWorkspaces()
        {
            foreach(var complexWorkspaceProgress in _progressService.RegionProgress.ComplexWorkspaceProgresses)
            {
                var processingWorkspaceModel =
                    _workspaceFactory.CreateComplexWorkspaceModel(complexWorkspaceProgress.Guid, complexWorkspaceProgress.ComplexType);
                _complexWorkspaceModels[complexWorkspaceProgress.Guid] = processingWorkspaceModel;
                processingWorkspaceModel.LevelUpdated += OnComplexUpdateLevel;
                processingWorkspaceModel.Builded += OnComplexBuilded;
            }
        }

        private void OnComplexBuilded(IBuilding building)
        {
            if(building is ComplexWorkspaceModel complexWorkspaceModel)
                UpdateComplexCapacities(complexWorkspaceModel.CurrentLevelData);
        }

        private void OnComplexUpdateLevel(ILevelsBuilding levelsBuilding)
        {
            if(levelsBuilding is ComplexWorkspaceModel complex)
                UpdateComplexCapacities(complex.CurrentLevelData);
        }

        private void UpdateComplexCapacities(ComplexLevelData complexLevelData)
        {
            foreach(var resourceCapacity in complexLevelData.ResourcesCapacity)
                CapacityChanged?.Invoke(resourceCapacity.ResourceType, resourceCapacity.Capacity);
        }
    }
}