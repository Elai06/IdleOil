using UnityEngine;

namespace Utils.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class AutoSizePanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_rect == null)
            {
                _rect = gameObject.GetComponent<RectTransform>();
            }
        }
#endif
        private void OnEnable()
        {
            _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _rect.rect.width);
        }
    }
}
