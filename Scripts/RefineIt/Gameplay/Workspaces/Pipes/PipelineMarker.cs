using Gameplay.Workspaces.Workers.Transport;
using UnityEngine;

namespace Gameplay.Workspaces.Pipes
{
    public class PipelineMarker : MonoBehaviour
    {
        public string Guid;
        public Transform ImportPoint;
        public Transform ExportPoint;
        public TransportType TransportType;
        public int Duration;
        
        private void OnDrawGizmosSelected()
        {
            if (ImportPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(ImportPoint.position, 0.2f);
            }

            if (ExportPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(ExportPoint.position, 0.2f);
            }
        }
    }
}