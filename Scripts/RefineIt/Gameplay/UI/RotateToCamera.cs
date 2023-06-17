using UnityEngine;

namespace Gameplay.UI
{
    public class RotateToCamera : MonoBehaviour
    {
        [SerializeField] private bool _isStatic;
        
        private Transform _camera;

        private void Awake()
        {
            _camera = Camera.main.transform;
        }

        private void Start()
        {
            Rotate();
        }

        void Update()
        {
            transform.LookAt(_camera);
            // if(_isStatic) return;
            //
            // if (transform.rotation != _camera.rotation)
            // {
            //     Rotate();
            // }
        }

        private void Rotate()
        {
            transform.rotation = _camera.rotation;
        }
    }
}
