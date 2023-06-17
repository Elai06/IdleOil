using Gameplay.Orders;
using Gameplay.Region.Storage;
using Gameplay.Workspaces.CrudeOilStorage;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using Infrastructure.StaticData.ProcessingWorkspace;
using UnityEngine;

namespace Gameplay.Workspaces
{
    public interface IWorkspaceFactory
    {
        void Initialize(IRegionStorage storageModel);
        MiningWorkSpaceModel CreateMiningWorkSpaceModel(Vector3Int guid);
        ProcessingWorkspaceModel CreateProcessingWorkspaceModel(ProcessingType id, Vector3Int guid);
        StorageOilCrudeModel CreateStorageOilCrudeWorkspaceModel(Vector3Int guid);
        ComplexWorkspaceModel CreateComplexWorkspaceModel(Vector3Int guid, ComplexType complexType);
    }
}