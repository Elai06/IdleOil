using System;
using System.Net.NetworkInformation;
using Infrastructure.Windows;
using UnityEngine;
using YandexMobileAds;
using Zenject;

namespace Infrastructure.YandexAds
{
    public class YandexAdsService : MonoBehaviour, IAdsService
    {
        public event Action VideoShown;

        private const string ADS_ID_EXAMPLE = "demo-rewarded-yandex";

        [SerializeField] private YandexMobileAdsRewardedAd _adsRewarded;

        private IWindowService _windowService;

        [Inject]
        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }

        private void Awake()
        {
            PreloadAd();
            _adsRewarded.Initialize(_windowService);
        }

        public bool IsAdLoaded => _adsRewarded != null && _adsRewarded.IsLoaded();

        public void SetAdsRewardId(string rewardVideoId)
        {
            _adsRewarded.InitializeId(ADS_ID_EXAMPLE);
            PreloadAd();
        }

        public void SetUserConsent(bool isAgree)
        {
            MobileAds.SetUserConsent(isAgree);
        }

        public bool ShowAdsVideo()
        {
#if UNITY_EDITOR

            return false;
#endif

            if (NetworkInterface.GetIsNetworkAvailable() == false)
            {
                return false;
            }
            
            if (!_adsRewarded.ShowRewardedAd())
            {
                return false;
            }

            _adsRewarded.VideoShown += OnVideoShown;

            return true;
        }

        private void PreloadAd()
        {
#if UNITY_IOS || UNITY_ANDROID

            _adsRewarded.ClearAd();
            _adsRewarded.RequestRewardedAd();
#endif
        }

        private void OnVideoShown()
        {
            _adsRewarded.VideoShown -= OnVideoShown;
            VideoShown?.Invoke();
            PreloadAd();
        }
    }
}