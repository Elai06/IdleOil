using System;
using System.Collections;
using Gameplay.Tilemaps;
using Gameplay.Tilemaps.Services;
using UnityEngine;
using Zenject;

namespace Gameplay.CameraProject
{
    public class CameraZoomController : MonoBehaviour
    {
        [SerializeField] private float _zoomSpeed = 5.0f;
        [SerializeField] private float _minZoom = 2.0f;
        private float _saveZoom = 10.0f;

        private Vector3Int zoomTargetPosition;
        private bool _isZoom;
        private bool _isZoomed = false;
        private Coroutine zoomCoroutine;

        private ITilemapController _tilemapController;
        private TilemapMarker _tilemapMarker;
        private Camera cameraMain;

        [Inject]
        public void Construct(ITilemapController tilemapController)
        {
            _tilemapController = tilemapController;
        }

        public void Initialize()
        {
            cameraMain = Camera.main;
            _tilemapController.ClickOnBuild += ZoomIn;
            _tilemapMarker = FindObjectOfType<TilemapMarker>();
        }

        private void OnDisable()
        {
            _tilemapController.ClickOnBuild -= ZoomIn;
        }

        private void StartZoomCoroutine(Vector3Int targetPosition)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }

            zoomTargetPosition = targetPosition;
            zoomCoroutine = StartCoroutine(ZoomCoroutine());
        }

        private void StopZoomCoroutine()
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
                zoomCoroutine = null;
            }
        }

        private IEnumerator ZoomCoroutine()
        {
            var currentZoom = cameraMain.orthographicSize;
            Vector3 currentPos = cameraMain.transform.position;
            Vector3 targetPosition = _tilemapMarker.Grid.CellToWorld(zoomTargetPosition);

            while (!_isZoomed || currentPos != targetPosition)
            {
                if (_isZoom)
                {
                    currentPos = Vector3.MoveTowards(currentPos, targetPosition, Time.deltaTime * _zoomSpeed);
                    currentZoom = Mathf.MoveTowards(currentZoom, _minZoom, Time.deltaTime * _zoomSpeed);
                    if (currentZoom == _minZoom)
                    {
                        _isZoomed = true;
                    }
                    
                    cameraMain.transform.position = currentPos;
                }
                else
                {
                    currentZoom = Mathf.MoveTowards(currentZoom, _saveZoom, Time.deltaTime * _zoomSpeed);
                    if (currentZoom == _saveZoom)
                    {
                        _isZoomed = true;
                    }
                }

                cameraMain.orthographicSize = Mathf.Clamp(currentZoom, _minZoom, _saveZoom);

                yield return null;
            }

            _isZoomed = false;
            zoomCoroutine = null;
        }

        private void ZoomIn(Vector3Int targetPosition)
        {
            _isZoom = true;
            _saveZoom = cameraMain.orthographicSize;
            StartZoomCoroutine(targetPosition);
        }

        public void ZoomOut()
        {
            _isZoom = false;
            StartZoomCoroutine(zoomTargetPosition);
        }

        public void CancelZoom()
        {
            StopZoomCoroutine();
        }
    }
}