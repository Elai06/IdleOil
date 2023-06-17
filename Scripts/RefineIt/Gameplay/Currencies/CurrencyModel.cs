using System;
using UnityEngine.AddressableAssets;

namespace Gameplay.Currencies
{
    public class CurrencyModel
    {
        private readonly CurrencyStaticData _currencyStaticData;
        private readonly CurrencyData _currencyData;

        public CurrencyModel(CurrencyStaticData currencyStaticData, CurrencyData currencyData)
        {
            var b = 0;
            _currencyData = currencyData;
            _currencyStaticData = currencyStaticData;
        }

        public event Action AmountUpdated;
        public int Amount => _currencyData.Amount;
        public AssetReferenceSprite SpriteReference => _currencyStaticData.Sprite;

        public void Add(int amount)
        {
            _currencyData.Add(amount);
            AmountUpdated?.Invoke();
        }

        public void Consume(int amount)
        {
            _currencyData.Remove(amount);
            AmountUpdated?.Invoke();
        }

        public bool CanConsume(int amount)
        {
            return Amount >= amount;
        }
    }
}