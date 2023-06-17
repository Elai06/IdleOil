using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.PromoCode
{
    public class PromoCodeView : MonoBehaviour
    {
        private const string FAILED = "Промокод введён неправильно";
        private const string Expired = "Срок промокода истёк";
        private const string SUCCESSFUL = "Промокод успешно введён";
        private const string Entered = "Промокод уже был введён";

        public event Action<string> EnterPromoCodeButton;

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _enterPromoCodeButton;
        [SerializeField] private TextMeshProUGUI _promoCodeStatusField;

        [SerializeField] private Transform _upTransform;

        private Vector3 _position;

        private void Start()
        {
            _enterPromoCodeButton.onClick.AddListener(EnterPromoCode);

            _inputField.text = "";

            _position = transform.parent.parent.position;

#if !UNITY_EDITOR
            _inputField.onSelect.AddListener(OnSelect);
            _inputField.onEndEdit.AddListener(OnEnd);
#endif
        }

        private void OnSelect(string arg0)
        {
            transform.parent.parent.position = _upTransform.position;
        }

        private void OnEnd(string arg0)
        {
            transform.parent.parent.position = _position;
        }

        public void ShowPromoCodeStatus(PromoCodeStatusType status)
        {
            switch (status)
            {
                case PromoCodeStatusType.Failed:
                    _promoCodeStatusField.text = FAILED;
                    _promoCodeStatusField.color = Color.red;
                    break;
                case PromoCodeStatusType.Available:
                    _promoCodeStatusField.text = SUCCESSFUL;
                    _promoCodeStatusField.color = Color.green;
                    break;
                case PromoCodeStatusType.Expired:
                    _promoCodeStatusField.text = Expired;
                    _promoCodeStatusField.color = Color.red;
                    break;
                case PromoCodeStatusType.Entered:
                    _promoCodeStatusField.text = Entered;
                    _promoCodeStatusField.color = Color.red;
                    break;
            }
        }

        private void EnterPromoCode()
        {
            EnterPromoCodeButton?.Invoke(_inputField.text);
        }
    }
}