using System;

namespace Gameplay.Services.Timer
{
    public class Timer
    {
        public event Action<int> Tick;
        public event Action<Timer> Stopped;

        private int _tickDuration;

        public bool IsWork { get; set; } = true;
        public int TickDuration => _tickDuration;

        public Timer(int tickDuration)
        {
            _tickDuration = tickDuration;
        }

        public void NotifyAboutTick()
        {
            if (IsWork)
            {
                _tickDuration -= 1;
                Tick?.Invoke(_tickDuration);
                
                if (_tickDuration <= 0)
                {
                    Stop();
                }
            }
        }

        private void Stop()
        {
            Stopped?.Invoke(this);
            IsWork = false;
        }
    }
}