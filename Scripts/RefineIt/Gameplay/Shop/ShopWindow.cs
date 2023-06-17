using Gameplay.Store;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Shop
{
    public class ShopWindow : Window
    {
        [SerializeField] private ShopViewInitializer _shopViewInitializer;

        private IShopModel _shopModel;

        [Inject]
        public void Construct(IShopModel shopModel)
        {
            _shopModel = shopModel;
        }
        
        private void Start()
        {
            _shopViewInitializer.Initialize(_shopModel);
        }
    }
}