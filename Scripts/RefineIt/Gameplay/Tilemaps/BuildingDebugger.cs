using System.Collections.Generic;
using System.Text;
using Gameplay.Tilemaps.Buildings;
using Gameplay.Tilemaps.Factories;
using Gameplay.Workspaces.ComplexWorkspace;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.ProcessingWorkspace;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gameplay.Tilemaps
{
    public class BuildingDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        public TilemapMarker TilemapMarker;

        private IBuildingService _buildingService;

        [Inject]
        private void Construct(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        private List<ProcessingWorkspaceModel> GetProcesses()
        {
       
            var processingWorkspaceModels = new List<ProcessingWorkspaceModel>();
            foreach(var building in _buildingService.Buildings)
            {
                if(building is ProcessingWorkspaceModel model)
                    processingWorkspaceModels.Add(model);
            }
            return processingWorkspaceModels;
        }
        
        private List<ComplexWorkspaceModel> GetComplexies()
        {
       
            var complexies = new List<ComplexWorkspaceModel>();
            foreach(var building in _buildingService.Buildings)
            {
                if(building is ComplexWorkspaceModel model)
                    complexies.Add(model);
            }
            return complexies;
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying == false)
                return;

            Handles.BeginGUI();
            Gizmos.color = Color.red;
            foreach(var processingWorkspaceModel in GetProcesses())
            {
                var cellToWorld = TilemapMarker.Grid.CellToWorld(processingWorkspaceModel.Guid);
                Gizmos.DrawSphere(cellToWorld, 0.1f);
                var cam = SceneView.lastActiveSceneView.camera;
                var worldToScreenPoint = cam.WorldToScreenPoint(cellToWorld);
                GUI.Label(new Rect(worldToScreenPoint.x, worldToScreenPoint.y, 1000, 100), GetCapacities(processingWorkspaceModel));
            }          
            
            foreach(var complexWorkspaceModel in GetComplexies())
            {
                var cellToWorld = TilemapMarker.Grid.CellToWorld(complexWorkspaceModel.Guid);
                Gizmos.DrawSphere(cellToWorld, 0.1f);
                var cam = SceneView.lastActiveSceneView.camera;
                var worldToScreenPoint = cam.WorldToScreenPoint(cellToWorld);
                GUI.Label(new Rect(worldToScreenPoint.x, worldToScreenPoint.y, 1000, 100), GetCapacities(complexWorkspaceModel));
            }

            Handles.EndGUI();
        }

        private string GetCapacities(ProcessingWorkspaceModel processingWorkspaceModel)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(" Input: ");
            foreach(var resourceProgress in processingWorkspaceModel.InputResourceStorage.Resources.Values)
            {
                stringBuilder.Append($"{resourceProgress.ResourceType} : {resourceProgress.Amount}");
            }

            stringBuilder.Append("\n OutPut: ");
            foreach(var resourceProgress in processingWorkspaceModel.OutputResourcesStorage.Resources.Values)
            {
                stringBuilder.Append($"{resourceProgress.ResourceType} : {resourceProgress.Amount}");
            }

            return stringBuilder.ToString();
        }      
        
        private string GetCapacities(ComplexWorkspaceModel complexWorkspaceModel)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(" Complex: ");
            foreach(var resourceProgress in complexWorkspaceModel.ImportStorage.Resources.Values)
            {
                stringBuilder.Append($"{resourceProgress.ResourceType} : {resourceProgress.Amount}");
            }
            return stringBuilder.ToString();
        }
#endif
    }

}