using System;
using Infrastructure.Windows.MVVM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Gameplay.Currencies
{
    public class CurrencyView : MonoBehaviour, IInteractableView
    {
        [SerializeField] private TMP_Text _amount;
        [SerializeField] private Image _image;

        public event Action Clicked;
        
        public void ViewAmount(int amount)
        {
            _amount.text = amount.ToFormattedBigNumber();
        }

        public void ViewIcon(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}