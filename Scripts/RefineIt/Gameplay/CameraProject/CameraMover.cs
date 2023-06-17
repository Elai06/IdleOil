using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private GameObject _bounds1;
    [SerializeField] private GameObject _bounds2;
    [SerializeField] private float _cameraSpeed = 0.25f;
    [SerializeField] private float _dragSpeed = 0.15f;

    private float _zoomSpeed = 0.05f;
    private float _maxZoom = 5f;
    private float _minZoom = 10f;

    private Camera _camera;
    private Vector3 _lastMousePosition;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR

        if (EventSystem.current.IsPointerOverGameObject())
            return;
#else
                if (Input.touches.Length > 0 && EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return;
#endif

        float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        if (zoomDelta != 0)
        {
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - zoomDelta, _maxZoom, _minZoom);
        }

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;
                float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
                float touchDeltaMag = (touch1.position - touch2.position).magnitude;
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + deltaMagnitudeDiff * _zoomSpeed,
                    _maxZoom, _minZoom);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - _lastMousePosition;
            _camera.transform.position -= new Vector3(delta.x, delta.y, 0) * (_dragSpeed * Time.deltaTime);
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X") * _cameraSpeed * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * _cameraSpeed * Time.deltaTime;
            _camera.transform.Translate(new Vector3(x, y, 0));
        }

        Vector2 vec1 = new Vector2(_bounds1.transform.position.x, _bounds1.transform.position.y);
        Vector2 vec2 = new Vector2(_bounds2.transform.position.x, _bounds2.transform.position.y);
        SetBounds(vec1, vec2);
    }

    public void SetBounds(Vector2 minBounds, Vector2 maxBounds)
    {
        // ограничиваем движение камеры по осям x и y
        if (_camera.transform.position.x < minBounds.x)
            _camera.transform.position =
                new Vector3(minBounds.x, _camera.transform.position.y, _camera.transform.position.z);
        else if (_camera.transform.position.x > maxBounds.x)
            _camera.transform.position =
                new Vector3(maxBounds.x, _camera.transform.position.y, _camera.transform.position.z);

        if (_camera.transform.position.y < minBounds.y)
            _camera.transform.position =
                new Vector3(_camera.transform.position.x, minBounds.y, _camera.transform.position.z);
        else if (_camera.transform.position.y > maxBounds.y)
            _camera.transform.position =
                new Vector3(_camera.transform.position.x, maxBounds.y, _camera.transform.position.z);
    }
}