using System.Text;
using Gameplay.Workspaces.MiningWorkspace;
using Gameplay.Workspaces.Workers.Transport;
using Zenject;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gameplay.Tilemaps
{
    public class TransportDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        private ITransportSpawner _transportSpawner;

        [Inject]
        private void Construct(ITransportSpawner buildingService)
        {
            _transportSpawner = buildingService;
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying == false)
                return;

            Handles.BeginGUI();
            foreach(var transport in _transportSpawner.Transports)
            {
                var cam = SceneView.lastActiveSceneView.camera;
                var worldToScreenPoint = cam.WorldToScreenPoint(transport.Mover.transform.position);
                GUI.Label(new Rect(worldToScreenPoint.x, worldToScreenPoint.y, 1000, 100), GetTransportCapacities(transport));
            }

            Handles.EndGUI();
        }

        private string GetTransportCapacities(TransportModel transportModel)
        {
            var stringBuilder = new StringBuilder();
            foreach(var resourceProgress in transportModel.Storage.Resources.Values)
            {
                stringBuilder.Append($"{resourceProgress.ResourceType} : {resourceProgress.Amount}");
            }

            return stringBuilder.ToString();
        }
#endif
    }
}