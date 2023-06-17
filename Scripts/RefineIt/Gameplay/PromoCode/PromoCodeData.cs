using System;
using System.Collections.Generic;
using Gameplay.Currencies;
using Gameplay.Workspaces.MiningWorkspace;

namespace Gameplay.PromoCode
{
    [Serializable]
    public class PromoCodeData
    {
        public string Key;
        public string FromDateTime;
        public string ToTheTime;
        public List<CurrencyData> Currency;
    }
}