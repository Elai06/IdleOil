using System;
using System.Collections.Generic;
using Gameplay.Order;
using Gameplay.Orders;
using Gameplay.Personnel;
using Gameplay.Region.Data;
using Gameplay.Workers;
using Gameplay.Workspaces.CrudeOilStorage;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Gameplay.Workspaces.Workers.Transport;
using Gameplay.Workspaces.Workers.Wagon;
using Infrastructure.StaticData.ProcessingWorkspace;
using UnityEngine;
using StorageProgress = Gameplay.Region.StorageProgress;

namespace Infrastructure.PersistenceProgress
{
    [Serializable]
    public class RegionProgress
    {
        public RegionType RegionType;

        public List<MiningWorkspaceProgress> MiningWorkSpaceProgresses;
        public List<ProcessingWorkspaceProgress> ProcessingWorkspaceProgresses;
        public List<ComplexWorkspaceProgress> ComplexWorkspaceProgresses;
        public List<StorageOilCrudeProgress> StorageOilCrudeWorkspaceProgresses;
        public List<TransportProgress> TransportProgresses;
        public List<TransportOrderProgress> WagonProgresses;
        public List<WorkerProgress> WorkerProgresses;

        public TimeOrderProgresses TimeOrderProgresses;
        public StorageProgress Storage;
        public OrdersProgress OrdersProgress;

        public bool IsSelected;
        public bool IsTutorial;
        public bool IsLastOrderTutorial;

        public RegionProgress(RegionType regionType, bool isSelected)
        {
            RegionType = regionType;
            MiningWorkSpaceProgresses = new List<MiningWorkspaceProgress>();
            ProcessingWorkspaceProgresses = new List<ProcessingWorkspaceProgress>();
            StorageOilCrudeWorkspaceProgresses = new List<StorageOilCrudeProgress>();
            ComplexWorkspaceProgresses = new List<ComplexWorkspaceProgress>();
            TransportProgresses = new List<TransportProgress>();
            TimeOrderProgresses = new TimeOrderProgresses();
            WagonProgresses = new List<TransportOrderProgress>();
            WorkerProgresses = new List<WorkerProgress>();

            Storage = new StorageProgress();
            OrdersProgress = new OrdersProgress();
            IsTutorial = true;
            IsLastOrderTutorial = false;

            IsSelected = isSelected;
        }

        public MiningWorkspaceProgress GetOrCreateMiningWorkspaceProgress(Vector3Int guid)
        {
            var miningWorkspaceProgress = MiningWorkSpaceProgresses.Find(x => x.Guid == guid);
            if (miningWorkspaceProgress == null)
            {
                MiningWorkspaceProgress progress = new(guid);
                MiningWorkSpaceProgresses.Add(progress);

                return progress;
            }

            return miningWorkspaceProgress;
        }

        public ProcessingWorkspaceProgress GetOrCreateProcessingWorkspaceProgress(Vector3Int guid, ProcessingType type)
        {
            var processingWorkspaceProgress =
                ProcessingWorkspaceProgresses.Find(x => x.Guid == guid);
            if (processingWorkspaceProgress == null)
            {
                ProcessingWorkspaceProgress workspaceProgress = new(guid, type);
                ProcessingWorkspaceProgresses.Add(workspaceProgress);
                return workspaceProgress;
            }

            return processingWorkspaceProgress;
        }

        public ComplexWorkspaceProgress GetOrCreateComplexWorkspaceProgress(Vector3Int guid)
        {
            var complexWorkspaceProgress = ComplexWorkspaceProgresses.Find(x => x.Guid == guid);

            if (complexWorkspaceProgress == null)
            {
                ComplexWorkspaceProgress progress = new(guid);
                ComplexWorkspaceProgresses.Add(progress);
                return progress;
            }

            return complexWorkspaceProgress;
        }

        public StorageOilCrudeProgress GetOrCreateStorageOilCrudeProgress(Vector3Int guid)
        {
            var storageOilCrudeProgress =
                StorageOilCrudeWorkspaceProgresses.Find(x => x.Guid == guid);

            if (storageOilCrudeProgress == null)
            {
                StorageOilCrudeProgress progress = new(guid);
                StorageOilCrudeWorkspaceProgresses.Add(progress);
                return progress;
            }

            return storageOilCrudeProgress;
        }

        public TransportProgress GetTransport(string guid, TransportType type)
        {
            var transportProgress = TransportProgresses.Find(x => x.Guid == guid);

            if (transportProgress == null)
            {
                TransportProgress progress = new(guid, type);
                TransportProgresses.Add(progress);
                return progress;
            }

            return transportProgress;
        }

        public TransportOrderProgress GetWagon(string guid)
        {
            var transportOrderProgress = WagonProgresses.Find(x => x.Guid == guid);

            if (transportOrderProgress == null)
            {
                TransportOrderProgress progress = new(guid);
                WagonProgresses.Add(progress);
                return progress;
            }

            return transportOrderProgress;
        }

        public WorkerProgress GetOrCreatePersonnelProgress(WorkerType type)
        {
            var workerProgress = WorkerProgresses.Find(x => x.WorkerType == type);

            if (workerProgress == null)
            {
                WorkerProgress progress = new(type);
                WorkerProgresses.Add(progress);
                return progress;
            }

            return workerProgress;
        }
    }
}