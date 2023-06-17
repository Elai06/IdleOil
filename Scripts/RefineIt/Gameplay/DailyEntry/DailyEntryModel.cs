using System;
using System.Collections.Generic;
using System.Globalization;
using Gameplay.Currencies;
using Gameplay.DailyEntry.UI;
using Gameplay.Services.Timer;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.DailyEntry
{
    public class DailyEntryModel : IDailyEntryModel
    {
        private IWindowService _windowService;
        private IStaticDataService _staticDataService;
        private IProgressService _progressService;
        private CurrenciesModel _currenciesModel;
        private TimerService _timerService;
        
        DailyEntryModel(IWindowService windowService, IStaticDataService staticDataService, IProgressService progressService, CurrenciesModel currenciesModel, TimerService timerService)
        {
            _windowService = windowService;
            _staticDataService = staticDataService;
            _progressService = progressService;
            _currenciesModel = currenciesModel;
            _timerService = timerService;
        }

        public void Initializer()
        {
            _timerService.TimeModels[TimerType.DailyTimer.ToString()].Stopped += StoppedDailyTimer;
            InitializeCurrentDay();
            InitializeProgressDaily();
            _windowService.Open(WindowType.DailyEntry);
        }

        public DailyEntryType GetCurDailyEntryType()
        {
            DailyEntryType day = (DailyEntryType)Enum.Parse(typeof(DailyEntryType),
                _progressService.PlayerProgress.CurrentDailyEntry.ToString());
            return day;
        }

        public DailyEntryComponentData GetComponentData(DailyEntryType day)
        {
            return _staticDataService.DailyEntryData.DailyEntry.Find(x => x.Day == day);;
        }
        
        public DailyEntryProgress GetProgress(DailyEntryType day)
        {
            return _progressService.PlayerProgress.GetOrCreateDailyEntry(day);;
        }

        public void TakeReward(DailyEntryType day , CurrencyType currencyType, int reward)
        {
            _currenciesModel.Add(currencyType,reward);
            var progress =_progressService.PlayerProgress.DailyEntryProgresses.Find(x 
                => x.Day == day);
            progress.IsTake = true;
            progress.IsVisableReward = false;
        }

        private void InitializeCurrentDay()
        {
            DateTime lastSession = DateTime.MinValue;
            DateTime minValue = DateTime.MinValue;

            if (_progressService.PlayerProgress.LastSession != null)
            {
                lastSession = DateTime.Parse(_progressService.PlayerProgress.LastSession);
            }
            
            var dateNow = DateTime.Now;
            lastSession = lastSession.Date.AddHours(minValue.Hour);
            dateNow = dateNow.Date.AddHours(minValue.Hour);

            if ((dateNow - lastSession).TotalDays >= 1)
            {
                if(_progressService.PlayerProgress.CurrentDailyEntry < 7)
                    _progressService.PlayerProgress.CurrentDailyEntry++;
                else
                {
                    _progressService.PlayerProgress.CurrentDailyEntry = 1;
                    _progressService.PlayerProgress.ReCreateProgress();
                }
            }
        }

        private void InitializeProgressDaily()
        {
            foreach (var progress in _progressService.PlayerProgress.DailyEntryProgresses)
            {
                _progressService.PlayerProgress.GetOrCreateDailyEntry(progress.Day);
                if (progress.Day == GetCurDailyEntryType() && !progress.IsTake)
                {
                    progress.IsVisableReward = true;
                }

                if (progress.Day < GetCurDailyEntryType() && !progress.IsTake)
                {
                    var component = GetComponentData(progress.Day);
                    TakeReward(progress.Day, component.rewardType, int.Parse(component.reward));
                }
            }
        }
        
        
        private void StoppedDailyTimer(TimeModel timer)
        {
            if(_progressService.PlayerProgress.CurrentDailyEntry < 7)
                _progressService.PlayerProgress.CurrentDailyEntry++;
            else
            {
                _progressService.PlayerProgress.CurrentDailyEntry = 1;
                _progressService.PlayerProgress.ReCreateProgress();
            }
            InitializeProgressDaily();
            _windowService.Open(WindowType.DailyEntry);
        }
    }
}