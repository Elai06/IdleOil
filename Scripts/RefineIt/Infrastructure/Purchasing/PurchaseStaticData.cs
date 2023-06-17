using System.Collections.Generic;
using Constants;
using Gameplay.Currencies;
using UnityEngine;

namespace Infrastructure.Purchasing
{
    [CreateAssetMenu(fileName = FILE, menuName = MENU)]
    public class PurchaseStaticData : ScriptableObject
    {
        private const string CATEGORY = "StaticData";
        private const string TITLE = "Purchase";
        private const string FILE = TITLE + CATEGORY;
        private const string MENU = Names.PATH + "/" + CATEGORY + "/" + TITLE;

        [SerializeField] private string _title;
        [SerializeField] private bool _isInApp;
        [SerializeField] private List<CurrencyData> _price;
        [SerializeField] private List<CurrencyData> _currencies;

        public string Title => _title;
        public bool IsInApp => _isInApp;
        public List<CurrencyData> Price => _price;
        public List<CurrencyData> Currencies => _currencies;
    }
}