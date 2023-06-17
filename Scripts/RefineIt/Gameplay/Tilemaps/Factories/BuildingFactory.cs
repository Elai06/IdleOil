using System;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Data;
using Gameplay.Workspaces;
using Gameplay.Workspaces.Buildings.States;
using Gameplay.Workspaces.ComplexWorkspace;
using Infrastructure.StaticData.ProcessingWorkspace;

namespace Gameplay.Tilemaps.Factories
{
    public class BuildingFactory : IBuildingFactory
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly IBuildingService _buildingService;

        public BuildingFactory(IWorkspaceService workspaceService, IBuildingService buildingService)
        {
            _workspaceService = workspaceService;
            _buildingService = buildingService;
        }

        public IBuilding Create(BuildingData buildingData)
        {
            IBuilding building;
            switch(buildingData.Type)
            {
                case BuildingType.Mining:
                    building = _workspaceService.GetMiningModel(buildingData.Position);
                    break;
                case BuildingType.Complex:
                    if(Enum.TryParse(buildingData.Id, out ComplexType complexType))
                        building = _workspaceService.GetComplexModel(buildingData.Position, complexType);
                    else
                        throw new InvalidOperationException($"Building Data incorrect Id : {buildingData.Id}");
                    break;
                case BuildingType.Process:
                    if(Enum.TryParse(buildingData.Id, out ProcessingType processingType))
                        building = _workspaceService.GetProcessModel(buildingData.Position, processingType);
                    else
                        throw new InvalidOperationException($"Building Data incorrect Id : {buildingData.Id}");
                    break;
                case BuildingType.Storage:
                    building = _workspaceService.GetOilCrudeModel(buildingData.Position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                    break;
            }
            
            _buildingService.AddBuilding(building);

            if(buildingData.BuildFromStart)
                building.Build();
            
            if(building.IsBuilded)
            {
                building.InitializeState<IdleState>();
                return building;
            }

            if(building.IsConstructing)
            {
                building.InitializeState<ConstructionState>();
                return building;
            }

            building.InitializeState<NotBuildedState>();
            return building;
        }
    }
}