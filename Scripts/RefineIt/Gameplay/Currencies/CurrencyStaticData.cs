using Constants;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Currencies
{
    [CreateAssetMenu(fileName = FILE, menuName = MENU)]
    public class CurrencyStaticData : ScriptableObject
    {
        private const string CATEGORY = "StaticData";
        private const string TITLE = "Currency";
        private const string FILE = TITLE + CATEGORY;
        private const string MENU = Names.PATH + "/" + CATEGORY + "/" + TITLE;
        
        public CurrencyType CurrencyType;
        public AssetReferenceSprite Sprite;
    }
}