using UnityEngine;

namespace Gameplay.Workspaces.Workers.Transport
{
    public class DirectionSpriteView : MonoBehaviour
    {
        [SerializeField] private Vector3 _forward = new Vector3(2, -1.162791f, 0);
        [SerializeField] private TransportMover _curve;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private int _spriteInterval;

        private void OnValidate()
        {
            _spriteInterval = 360 / _sprites.Length;
        }

        private void Awake()
        {
            _spriteInterval = 360 / _sprites.Length;
        }

        public void FixedUpdate()
        {
            var spriteIndex = GetSpriteIndex();
            _spriteRenderer.sprite = _sprites[spriteIndex];
        }

        private int GetSpriteIndex()
        {
            var signedAngle = Vector2.SignedAngle(_forward, _curve.Direction());

            if(signedAngle < 0)
                signedAngle += 360;

            var spriteIndex = (int)signedAngle / _spriteInterval;
            if(spriteIndex == _sprites.Length)
                spriteIndex = 0;

            return spriteIndex;
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying)
                Debug.DrawRay(transform.position, _curve.Direction(), Color.red);
            Debug.DrawRay(transform.position, _forward, Color.green);
        }
    }
}