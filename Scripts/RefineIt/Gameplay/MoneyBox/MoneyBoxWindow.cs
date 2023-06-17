using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.MoneyBox
{
    public class MoneyBoxWindow : Window
    {
        [SerializeField] private MoneyBoxViewInitializer _moneyBoxViewInitializer;

        private MoneyBoxModel _moneyBoxModel;

        [Inject]
        private void Construct(MoneyBoxModel model)
        {
            _moneyBoxModel = model;
        }

        private void OnEnable()
        {
            _moneyBoxViewInitializer.Initialize(_moneyBoxModel);
        }
    }
}