using UnityEngine;

namespace Gameplay.Currencies.Coefficients
{
    [CreateAssetMenu(fileName = "CurrencyCoefficients", menuName = "Data/CurrencyCoefficients")]
    public class CurrencyCoefficientsStaticData : ScriptableObject
    {
        public CurrencyCoefficientsData[] CurrencyCoefficientsData;
    }
}