using System;
using Gameplay.Workspaces.Workers.Path;
using UnityEngine;

namespace Gameplay.Workspaces.Workers.Transport
{
    public class TransportMover : MonoBehaviour
    {
        private float _t = 0f;
        private BezierCurve _curve;

        public Vector3 Direction()
        {
            try
            {
                var value = _curve.GetPointAt(_t + 0.01f) - transform.position;
                return value;
            }
            catch(Exception e)
            {
                Debug.Log(e);
                return Vector3.zero;
            }
        }

        public void InitializePath(BezierCurve curve)
        {
            _curve = curve;
        }

        public void Move(float t)
        {
            _t = t;
            transform.position = _curve.GetPointAt(_t);
        }
    }
}