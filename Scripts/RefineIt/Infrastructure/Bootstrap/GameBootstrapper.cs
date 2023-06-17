using Gameplay.MobileNotification;
using Gameplay.Region.Data;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.Sates;
using UnityEngine;
using Zenject;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

namespace Infrastructure.Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        private IStateMachine _stateMachine;
        private IMobileNotification _mobileNotification;
        public RegionType RegionType;

        [Inject]
        private void Construct(IStateMachine stateMachine, IMobileNotification mobileNotification)
        {
            _stateMachine = stateMachine;
            _mobileNotification = mobileNotification;
        }

        private void Awake() =>
            DontDestroyOnLoad(gameObject);

        private void Start()
        {
            _stateMachine.Enter<BootstrapState, RegionType>(RegionType);

#if UNITY_IOS
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _stateMachine.Enter<ExitState>();
#if UNITY_ANDROID
                _mobileNotification.CreateOrderNotification();
                _mobileNotification.LongDontEnteredNotification();
#endif
            }
            else
            {
#if UNITY_ANDROID
                AndroidNotificationCenter.CancelAllNotifications();
#endif
            }
        }

        private void OnApplicationQuit()
        {
            _mobileNotification.CreateOrderNotification();
            _mobileNotification.LongDontEnteredNotification();
            _stateMachine.Enter<ExitState>();
        }
    }
}