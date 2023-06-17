/*
 * This file is a part of the Yandex Advertising Network
 *
 * Version for Android (C) 2019 YANDEX
 *
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at https://legal.yandex.com/partner_ch/
 */

using System;
using System.Net.NetworkInformation;
using Infrastructure.Windows;
using UnityEngine;
using Utils;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace Infrastructure.YandexAds
{
    public class YandexMobileAdsRewardedAd : MonoBehaviour
    {
        public event Action VideoShown;

        private IWindowService _windowService;
        private RewardedAd _rewardedAd;

        private string _adUnitId = "demo-rewarded-yandex";
        private bool _isSubscribed;


        public void Initialize(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void InitializeId(string id)
        {
            _adUnitId = id;
        }

        public void RequestRewardedAd()
        {
            if (_isSubscribed)
            {
                UnSubscribe();
            }

            _rewardedAd = new RewardedAd(_adUnitId);

            Subscribe();
            _rewardedAd.LoadAd(CreateAdRequest());
        }

        public void ClearAd()
        {
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                UnSubscribe();
            }
        }

        public bool ShowRewardedAd()
        {
            if (NetworkInterface.GetIsNetworkAvailable() == false)
            {
                return false;
            }
            
            if (_rewardedAd.IsLoaded())
            {
                _rewardedAd.Show();

                Debug.Log($"Reward is show {_rewardedAd.IsLoaded()}");
                return true;
            }

            _windowService.Open(WindowType.Loading);
            RequestRewardedAd();
            _rewardedAd.OnRewardedAdLoaded += OnAdLoaded;

            this.LogWarning("Rewarded Ad is not ready yet");
            return false;
        }

        private void OnAdLoaded(object sender, EventArgs e)
        {
            if (_windowService.CashedWindows[WindowType.Loading].gameObject.activeSelf)
            {
                return;
            }

            _windowService.Close(WindowType.Loading);
            _rewardedAd.Show();
            _rewardedAd.OnRewardedAdLoaded -= OnAdLoaded;
        }

        private void Subscribe()
        {
            if (_isSubscribed) return;

            _isSubscribed = true;

            _rewardedAd.OnRewardedAdLoaded += HandleRewardedAdLoaded;
            _rewardedAd.OnRewardedAdFailedToLoad += HandleRewardedAdFailedToLoad;
            _rewardedAd.OnReturnedToApplication += HandleReturnedToApplication;
            _rewardedAd.OnLeftApplication += HandleLeftApplication;
            _rewardedAd.OnAdClicked += HandleAdClicked;
            _rewardedAd.OnRewardedAdShown += HandleRewardedAdShown;
            _rewardedAd.OnRewardedAdDismissed += HandleRewardedAdDismissed;
            _rewardedAd.OnImpression += HandleImpression;
            _rewardedAd.OnRewarded += HandleRewarded;
            _rewardedAd.OnRewardedAdFailedToShow += HandleRewardedAdFailedToShow;
        }

        private void UnSubscribe()
        {
            if (!_isSubscribed) return;

            _rewardedAd.OnRewardedAdLoaded -= HandleRewardedAdLoaded;
            _rewardedAd.OnRewardedAdFailedToLoad -= HandleRewardedAdFailedToLoad;
            _rewardedAd.OnReturnedToApplication -= HandleReturnedToApplication;
            _rewardedAd.OnLeftApplication -= HandleLeftApplication;
            _rewardedAd.OnAdClicked -= HandleAdClicked;
            _rewardedAd.OnRewardedAdShown -= HandleRewardedAdShown;
            _rewardedAd.OnRewardedAdDismissed -= HandleRewardedAdDismissed;
            _rewardedAd.OnImpression -= HandleImpression;
            _rewardedAd.OnRewarded -= HandleRewarded;
            _rewardedAd.OnRewardedAdFailedToShow -= HandleRewardedAdFailedToShow;

            _isSubscribed = false;
        }

        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder().Build();
        }

        #region Rewarded Ad callback handlers

        private void HandleRewardedAdLoaded(object sender, EventArgs args)
        {
            this.Log("HandleRewardedAdLoaded event received");
        }

        private void HandleRewardedAdFailedToLoad(object sender, AdFailureEventArgs args)
        {
            this.Log(
                "HandleRewardedAdFailedToLoad event received with message: " + args.Message);
        }

        private void HandleReturnedToApplication(object sender, EventArgs args)
        {
            this.Log("HandleReturnedToApplication event received");
        }

        private void HandleLeftApplication(object sender, EventArgs args)
        {
            this.Log("HandleLeftApplication event received");
        }

        private void HandleAdClicked(object sender, EventArgs args)
        {
            this.Log("HandleAdClicked event received");
        }

        private void HandleRewardedAdShown(object sender, EventArgs args)
        {
            this.Log("HandleRewardedAdShown event received");
            VideoShown?.Invoke();
        }

        private void HandleRewardedAdDismissed(object sender, EventArgs args)
        {
            this.Log("HandleRewardedAdDismissed event received");
        }

        private void HandleImpression(object sender, ImpressionData impressionData)
        {
            var data = impressionData == null ? "null" : impressionData.rawData;
            this.Log("HandleImpression event received with data: " + data);
        }

        private void HandleRewarded(object sender, Reward args)
        {
            this.Log("HandleRewarded event received: amount = " + args.amount + ", type = " + args.type);
        }

        private void HandleRewardedAdFailedToShow(object sender, AdFailureEventArgs args)
        {
            this.Log("HandleRewardedAdFailedToShow event received with message: " + args.Message);
        }

        #endregion

        public bool IsLoaded()
        {
            return _rewardedAd != null && _rewardedAd.IsLoaded();
        }
    }
}