using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Services
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private float _dragSpeed = 10;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _borderPositionStart;
        [SerializeField] private Transform _borderPositionEnd;

        private CinemachineVirtualCamera _virtualCamera;
        private Camera _mainCamera;
        private Vector3 _startDragPosition;
        private Vector3 _targetOffset;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startDragPosition = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(0)) return;

            var pos = _mainCamera.ScreenToViewportPoint(Input.mousePosition - _startDragPosition);
            var cameraRotation = Quaternion.AngleAxis(_mainCamera.transform.rotation.eulerAngles.y, Vector3.up);
            var move = new Vector3(-pos.x * _dragSpeed, _target.transform.position.y, -pos.y * _dragSpeed);
            move = cameraRotation * move;

            MoveCamera(_target.position + move);
            _startDragPosition = Input.mousePosition;
        }

        public void MoveCamera(Vector3 moveTo)
        {
            var position = InterpolateNewPosition(moveTo);
            _target.position = position;
        }

        private Vector3 InterpolateNewPosition(Vector3 moveTo)
        {
            var clampVector = new Vector3();
            var minPosition = _borderPositionStart.position;
            var maxPosition = _borderPositionEnd.position;
            clampVector.x = Mathf.Clamp(moveTo.x, minPosition.x, maxPosition.x);
            clampVector.y = Mathf.Clamp(moveTo.y, minPosition.y, maxPosition.y);
            clampVector.z = Mathf.Clamp(moveTo.z, minPosition.z, maxPosition.z);
            return clampVector;
        }

        [Button]
        public void ChangeTarget(Transform target)
        {
            _target.position = target.position;
        }

        [Button]
        public void ChangeOffset(Vector3 offset)
        {
            _targetOffset = offset;

            var transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset = _targetOffset;
        }

        [Button]
        public void Init(Transform target, Vector3 offset)
        {
            ChangeTarget(target);
            ChangeOffset(offset);
        }

        private void OnDrawGizmos()
        {
            if (_borderPositionStart == null || _borderPositionEnd == null)
            {
                return;
            }

            var minPosition = _borderPositionStart.position;
            var maxPosition = _borderPositionEnd.position;

            //Draw borders
            Gizmos.DrawLine(minPosition,
                new Vector3(
                    minPosition.x,
                    minPosition.y,
                    maxPosition.z));
            Gizmos.DrawLine(minPosition,
                new Vector3(
                    maxPosition.x,
                    minPosition.y,
                    minPosition.z));
            Gizmos.DrawLine(maxPosition,
                new Vector3(
                    maxPosition.x,
                    maxPosition.y,
                    minPosition.z));
            Gizmos.DrawLine(maxPosition,
                new Vector3(
                    minPosition.x,
                    maxPosition.y,
                    maxPosition.z));
        }
    }
}