using System;
using Gameplay.Order;
using Gameplay.Services.Timer;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.Orders
{
    public class OrderAdsStatus
    {
        public event Action<bool> UpdateAdsButtonStatus;

        public bool IsCanShowAds { private set; get; }

        private readonly IProgressService _progressService;
        private readonly TimerService _timerService;

        public OrderAdsStatus(IProgressService progressService, TimerService timerService)
        {
            _progressService = progressService;
            _timerService = timerService;

            _timerService.StopTimer += OnAdsTimerStopped;
        }

        public void Initialize()
        {
            var timerProgress = _progressService.RegionProgress.TimeOrderProgresses.TimeProgresses
                .Find(x => x.ID == TimerType.ToAds.ToString());

            if (timerProgress != null)
            {
                _timerService.CreateTimer(TimerType.ToAds.ToString(), timerProgress.Time);
                return;
            }

            UpdateOrderAdsStatus();
        }

        public void UpdateOrderAdsStatus()
        {
            if (_progressService.RegionProgress.IsTutorial)
                return;
            
            IsCanShowAds = !_timerService.TimeModels.ContainsKey(TimerType.ToAds.ToString());

            foreach (var orderProgress in _progressService.RegionProgress.OrdersProgress.OrderProgresses)
            {
                orderProgress.isCanShowAds = IsCanShowAds;
            }

            UpdateAdsButtonStatus?.Invoke(IsCanShowAds);
        }

        private void OnAdsTimerStopped()
        {
            UpdateOrderAdsStatus();
        }
    }
}