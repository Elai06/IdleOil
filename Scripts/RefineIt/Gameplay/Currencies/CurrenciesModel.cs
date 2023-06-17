using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Quests;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Currencies
{
    public class CurrenciesModel : ICompositeModel<CurrencyModel>
    {
        public event Action<CurrencyType, int> CurrencyConsume;
        
        private readonly Dictionary<CurrencyType, CurrencyModel> _currencies = new();
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private IQuestModel _questModel;

        public CurrenciesModel(IProgressService progressService, IStaticDataService staticDataService)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
        }

        public IEnumerable<CurrencyModel> Models => _currencies.Values;
        
        public void Initialize(IQuestModel questModel)
        {
            _questModel = questModel;
            
            foreach(var currency in _progressService.PlayerProgress.Currencies)
            {
                var currencyData = _staticDataService.GetCurrencyData(currency.Type);
                _currencies[currencyData.CurrencyType] = new CurrencyModel(currencyData, currency);
            }
        }
        
        public void Add(CurrencyType type, int amount)
        {
            if (!_currencies.ContainsKey(type))
            {
                throw new InvalidOperationException($"No model for currency {type}");
            }
            
            if (type == CurrencyType.SoftCurrency)
            {
                _questModel.TaskDailyProgress(QuestsGuid.buySoft, amount);
                _questModel.TaskWeeklyProgress(QuestsGuid.buySoftWeek, amount);
            }

            _currencies[type].Add(amount);
        }
        
        public void Add(CurrencyData currency)
        {
            Add(currency.Type, currency.Amount);
        }

        public void Add(IEnumerable<CurrencyData> currencies)
        {
            foreach (var currency in currencies)
            {
                Add(currency);
            }
        }

        public void Consume(CurrencyType type, int amount)
        {
            if (!_currencies.ContainsKey(type))
            {
                throw new InvalidOperationException($"No model for currency {type}");
            }
            
            if (type == CurrencyType.SoftCurrency)
            {
                _questModel.TaskDailyProgress(QuestsGuid.sellSoft, amount);
                _questModel.TaskWeeklyProgress(QuestsGuid.sellSoftWeek, amount);
            }

            _currencies[type].Consume(amount);
            CurrencyConsume?.Invoke(type, amount);
        }

        public void Consume(CurrencyData currency)
        {
            Consume(currency.Type, currency.Amount);
        }
        
        public void Consume(IEnumerable<CurrencyData> currencies)
        {
            foreach (var currency in currencies)
            {
                Consume(currency);
            }
        }

        public int Get(CurrencyType type)
        {
            return _currencies[type]?.Amount ?? 0;
        }

        public bool Has(CurrencyType type, int cost)
        {
            return _currencies.ContainsKey(type) && _currencies[type].CanConsume(cost);
        }
        
        public bool Has(CurrencyData currency)
        {
            return Has(currency.Type, currency.Amount);
        }
        
        public bool Has(IEnumerable<CurrencyData> currencies)
        {
            return currencies.All(Has);
        }
    }
}