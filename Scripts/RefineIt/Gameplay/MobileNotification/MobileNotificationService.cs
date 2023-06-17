using System;
using Gameplay.Services.Timer;
using Infrastructure.PersistenceProgress;
using Utils.Extensions;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
using UnityEngine.iOS;
#endif


namespace Gameplay.MobileNotification
{
    public class MobileNotificationService : IMobileNotification
    {
        private readonly IProgressService _progressService;
        private readonly TimerService _timerService;

        public MobileNotificationService(IProgressService progressService, TimerService timerService)
        {
            _progressService = progressService;
            _timerService = timerService;
        }

        public void CreateOrderNotification()
        {
            foreach (var timeModel in _timerService.TimeModels.Values)
            {
                if (timeModel.TimeProgress.ID.Contains(TimerType.CreatedOrder.ToString()))
                {
                    var id = timeModel.TimeProgress.ID;
                    var time = DateTime.Now.AddSeconds(timeModel.TimeProgress.Time);
                    var title = "Появился новый заказ";
                    var text = "Зайдиту в игру, чтобы принять новый заказ";

#if UNITY_ANDROID
                    CreateAndroidPushNotification(id, time, title, text);
#elif UNITY_IOS
                    CreateIOSNotification(id, timeModel.TimeProgress.Time, title, text);
#endif
                }
            }
        }

        public void LongDontEnteredNotification()
        {
            var id = "LongDontEntered";
            var time = DateTime.Now.AddMinutes(15);
            var title = "Долго не было тебя в игре";
            var text = "Посмотри сколько накопилось ресурсов";
            var timeSpan = 15;

#if UNITY_ANDROID
            CreateAndroidPushNotification(id, time, title, text, true, timeSpan);
#elif UNITY_IOS
            CreateIOSNotification(id, FormatTime.MinutesIntFormat(15), title, text);
#endif
        }

        private void CreateAndroidPushNotification(string id, DateTime time, string title, string text,
            bool isRepeatInterval = false, int timeSpan = 0)
        {
#if UNITY_ANDROID
            AndroidNotificationChannel notificationChannel = new()
            {
                Id = id,
                Name = "Default channel",
                Description = "Description",
                Importance = Importance.High,
                EnableVibration = true,
                EnableLights = true
            };

            AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

            AndroidNotification notification = new()
            {
                Title = title,
                Text = text,
                LargeIcon = "icon_1",
                SmallIcon = "icon_2",
                FireTime = time
            };

            if (isRepeatInterval)
            {
                notification.RepeatInterval = new TimeSpan(0, timeSpan, 0);
            }

            AndroidNotificationCenter.SendNotification(notification, id);
#endif
        }

        private void CreateIOSNotification(string id, int time, string title, string text,
            bool isRepeatInterval = false)
        {
#if UNITY_IOS
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(0, 0, time),
                Repeats = isRepeatInterval
            };

            var notification = new iOSNotification()
            {
                Identifier = id,
                Title = title,
                Body = text,
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
#endif
        }
    }
}