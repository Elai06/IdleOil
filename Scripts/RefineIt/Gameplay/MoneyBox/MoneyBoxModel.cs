using System;
using Gameplay.Currencies;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Zenject;

namespace Gameplay.MoneyBox
{
    public class MoneyBoxModel : IInitializable, IDisposable
    {
        public event Action OnChange;

        private readonly CurrenciesModel _currenciesModel;
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;

        public MoneyBoxModel(CurrenciesModel currenciesModel,
            IProgressService progressService,
            IStaticDataService staticDataService)
        {
            _currenciesModel = currenciesModel;
            _progressService = progressService;
            _staticDataService = staticDataService;
        }

        public MoneyBoxProgressData ProgressData => _progressService.PlayerProgress.MoneyBoxProgressData;
        public MoneyBoxData Config => _staticDataService.MoneyBoxConfig.MoneyBoxesData[ProgressData.Level];

        public MoneyBoxData NextLevelConfig => _staticDataService.MoneyBoxConfig.MoneyBoxesData
            [IsMaxLevel ? ProgressData.Level : ProgressData.Level + 1];

        public bool IsMaxLevel => ProgressData.Level == _staticDataService.MoneyBoxConfig.MoneyBoxesData.Count - 1;

        public bool IsFillMoneyBox => ProgressData.Amount == Config.Capacities;


        public void Initialize()
        {
            _currenciesModel.CurrencyConsume += OnCurrenciesConsume;
        }

        public void Dispose()
        {
            _currenciesModel.CurrencyConsume -= OnCurrenciesConsume;
        }

        public void Buy()
        {
            ProgressData.IsBuy = true;
            OnChange?.Invoke();
        }

        public void Upgrade()
        {
            if (IsMaxLevel)
            {
                return;
            }

            ProgressData.Level++;
            OnChange?.Invoke();
        }

        public void GetHardCurrencies()
        {
            if (IsFillMoneyBox)
            {
                _currenciesModel.Add(CurrencyType.HardCurrency, ProgressData.Amount);
                ProgressData.Amount = 0;

                OnChange?.Invoke();
            }
        }

        private void OnCurrenciesConsume(CurrencyType type, int value)
        {
            if (type == CurrencyType.HardCurrency)
            {
                int moneyBoxValue = ProgressData.Amount + value / 3;
                ProgressData.Amount = moneyBoxValue < Config.Capacities ? moneyBoxValue : Config.Capacities;

                OnChange?.Invoke();
            }
        }
    }
}