using System;

namespace Infrastructure.YandexAds
{
    public interface IAdsService
    {
        public event Action VideoShown;

        bool IsAdLoaded { get; }
        
        bool ShowAdsVideo();
        void SetAdsRewardId(string rewardVideoId);
        void SetUserConsent(bool isAgree);
    }
}