using System;

namespace Gameplay.PromoCode
{
    [Serializable]
    public class PromoCodeProgress
    {
        public string Key;
        public string FromDateTime;
        public string ToTheTime;
        public bool IsEntered;
        public bool isExpired;
    }
}