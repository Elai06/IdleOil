using System.Collections.Generic;

namespace Gameplay.Shop
{
    public interface IShopModel
    {
        void Initialize();
        List<CurrencyProductModel> SoftProducts { get; }
        List<CurrencyProductModel> WorkerProducts { get; }
        List<PurchaseProductModel> HardProducts { get; }
        List<PurchaseProductModel> SetProducts { get; }
    }
}