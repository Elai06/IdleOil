using System.Collections.Generic;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.CrudeOilStorage;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.StaticData.ProcessingWorkspace;
using UnityEngine;

namespace Gameplay.Workspaces
{
    public interface IWorkspaceService : IResourceCapacity
    {
        void InitializeModels();
        MiningWorkSpaceModel GetMiningModel(Vector3Int id);
        StorageOilCrudeModel GetOilCrudeModel(Vector3Int id);
        ProcessingWorkspaceModel GetProcessModel(Vector3Int id, ProcessingType processingType);
        ComplexWorkspaceModel GetComplexModel(Vector3Int guid, ComplexType complexType);
        int GetComplexResourceCapacity(ResourceType resourceType);
        int GetStorageOilCrudeCapacity(ResourceType resourceType);
        Dictionary<Vector3Int, MiningWorkSpaceModel> MiningWorkspaces { get; }
        Dictionary<Vector3Int, ProcessingWorkspaceModel> ProcessingWorkspaces { get; }
        Dictionary<Vector3Int, ComplexWorkspaceModel> ComplexWorkspaceModels { get; }
    }
}