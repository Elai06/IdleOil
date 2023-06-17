using System;
using Gameplay.Currencies;

namespace Gameplay.Store
{
    [Serializable]
    public class Product
    {
        public CurrencyType CurrencyType;
        public int Value;
    }
}