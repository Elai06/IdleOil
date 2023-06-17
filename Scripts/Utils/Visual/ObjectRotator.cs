using UnityEngine;

namespace Utils.Visual
{
    public class ObjectRotator : MonoBehaviour
    {
        private enum EAxisType
        {
            X,
            Y,
            Z
        }

        [SerializeField] private Transform[] _objects;
        [SerializeField] private bool _ignoreTimescale = false;
        [SerializeField] private float _power;
        [SerializeField] private EAxisType _axis = EAxisType.X;

        private void LateUpdate()
        {
            RotateObjects();
        }

        public void SetTargets(Transform[] targets)
        {
            _objects = targets;
        }

        private void RotateObjects()
        {
            if (_objects != null && _objects.Length > 0)
            {
                for (var i = 0; i < _objects.Length; i++)
                {
                    var c = _objects[i];
                    if (c != null)
                    {
                        Rotate(c);
                    }
                }
            }
        }

        private void Rotate(Transform cooler)
        {
            var delta = (_ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime) * _power;
            switch (_axis) 
            {
                case (EAxisType.X):
                    cooler.Rotate(delta, 0f, 0f);
                    break;
                case (EAxisType.Y):
                    cooler.Rotate(0f, delta, 0f);
                    break;
                case (EAxisType.Z):
                    cooler.Rotate(0f, 0f, delta);
                    break;
            }
        }

        public void Clear()
        {
            _objects = null;
        }
    }
}
