using System;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.PromoCode
{
    [CreateAssetMenu(fileName = "PromoCodeConfig", menuName = "Data/PromoCodesConfig")]
    public class PromoCodesConfig : ScriptableObject
    {
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        [SerializeField] private int _characterNumber = 10;
        [SerializeField] private List<PromoCodeData> _promoCodesData = new();

        public List<PromoCodeData> PromoCodesData => _promoCodesData;

        [Button("Generate")]
        private void GeneratePromoCode()
        {
            string key = null;
            for (var i = 0; i < _characterNumber; i++)
            {
                var random = Random.Range(0, CHARS.Length);
                key += CHARS[random];
            }

            PromoCodeData promoCodeData = new()
            {
                Key = key,
                FromDateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture),
            };

            PromoCodesData.Add(promoCodeData);
        }
    }
}