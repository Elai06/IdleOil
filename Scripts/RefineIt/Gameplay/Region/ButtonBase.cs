using Infrastructure.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Region
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBase : MonoBehaviour
    {
        private Button _button;

        protected virtual void Awake() => 
            _button = GetComponent<Button>();

        private void OnEnable() => 
            _button.onClick.AddListener(OnClick);

        private void OnDisable() => 
            _button.onClick.RemoveListener(OnClick);

        public abstract void OnClick();
    }
}