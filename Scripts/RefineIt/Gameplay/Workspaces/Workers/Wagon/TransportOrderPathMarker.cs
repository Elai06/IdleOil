using Gameplay.Workspaces.Workers.Path;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Wagon
{
    public class TransportOrderPathMarker : MonoBehaviour
    {
        public string Guid;
        public TransportOrderType transportOrderType;
        public Transform StartPoint;
        public Transform EndPoint;
        public BezierCurve StartCurve;
        public BezierCurve EndCurve;

        private void OnDrawGizmosSelected()
        {
            if (StartPoint != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(StartPoint.position, 0.05f);
            }

            if (EndPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(EndPoint.position, 0.05f);
            }
        }
    }
}