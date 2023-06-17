using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Tilemaps.Services
{
    public class TilemapClickHandler : MonoBehaviour, ITileClickHandler
    {
        private Grid _grid;
        private bool _isTouchingUI;
        private bool _isTouchMoved;
        private readonly float _moveThreshold = 10f;

        public event Action<Vector3Int> Clicked;

        public void Initialize()
        {
            TilemapMarker tilemapMarker = FindObjectOfType<TilemapMarker>();
            _grid = tilemapMarker.Grid;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Click();
                }
            }
#else
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _isTouchingUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
                        break;
                    case TouchPhase.Moved:
                        if (touch.deltaPosition.magnitude > _moveThreshold)
                        {
                            _isTouchMoved = true;
                        }

                        break;
                    case TouchPhase.Ended:
                    {
                        if (!_isTouchingUI && !_isTouchMoved)
                        {
                            Click();
                        }

                        _isTouchingUI = false;
                        _isTouchMoved = false;
                        break;
                    }
                }
            }
#endif
        }

        private void Click()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 mouseFixedPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
            Vector3Int cellPos = _grid.WorldToCell(mouseFixedPos);
            Clicked?.Invoke(cellPos);
        }
    }
}