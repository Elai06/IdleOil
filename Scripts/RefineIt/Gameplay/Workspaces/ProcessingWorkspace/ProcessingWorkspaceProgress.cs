using System;
using System.Collections.Generic;
using Gameplay.Workers;
using Gameplay.Workspaces.Buildings.LevelBuildings;
using Gameplay.Workspaces.MiningWorkspace;
using Infrastructure.StaticData.ProcessingWorkspace;
using UnityEngine;
using StorageProgress = Gameplay.Region.StorageProgress;

namespace Gameplay.Workspaces.ProcessingWorkspace
{
    [Serializable]
    public class ProcessingWorkspaceProgress : LevelBuildingProgress
    {
        public ProcessingType Id;

        public StorageProgress RequiredStorage = new StorageProgress();
        public StorageProgress ProduceStorage = new StorageProgress();

        public ProcessingWorkspaceProgress(Vector3Int guid, ProcessingType type)
        {
            Guid = guid;
            Id = type;
        }
    }
}