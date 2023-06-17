using Gameplay.Workspaces.Workers.Transport;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Path
{
    public class TransportPathMarker : MonoBehaviour
    {
        public string Guid;
        public TransportType TransportType;
        public Transform ImportPoint;
        public Transform ExportPoint;
        public BezierCurve ImportCurve;
        public BezierCurve ExportCurve;

        private void OnDrawGizmosSelected()
        {
            if (ImportPoint != null)
            {
                Gizmos.color = Color.blue;
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