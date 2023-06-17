using System;
using UnityEngine;
using Utils;

namespace Gameplay.Currencies
{
    [Serializable]
    public class CurrencyData
    {
        [SerializeField] private CurrencyType _type;
        [SerializeField] private int _amount;

        public CurrencyType Type => _type;
        public int Amount => _amount;
        
        public CurrencyData(CurrencyType type, int amount = 0)
        {
            _type = type;
            _amount = amount;
        }

        public void Add(int amount)
        {
            _amount += amount;
            
            // ReSharper disable once InvertIf
            if (_amount < 0)
            {
                _amount = 0;
                
                this.LogWarning($"Currency {_type} can't be negative");
            }
        }

        public void Remove(int amount)
        {
            Add(-amount);
        }
    }
}