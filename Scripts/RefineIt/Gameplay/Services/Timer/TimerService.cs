using System;
using System.Collections.Generic;
using Infrastructure.PersistenceProgress;
using UnityEngine;
using Zenject;

namespace Gameplay.Services.Timer
{
    public class TimerService : MonoBehaviour
    {
        public event Action StopTimer;
        public event Action Tick;

        public float TimeSeconds { get; private set; }

        public readonly Dictionary<string, TimeModel> TimeModels = new();

        private IProgressService _progressService;

        [Inject]
        public void Construct(IProgressService progressService)
        {
            _progressService = progressService;
        }

        private void FixedUpdate()
        {
            TimeSeconds += Time.deltaTime;
            if (TimeSeconds >= 1)
            {
                Tick?.Invoke();
                TimeSeconds = 0;
            }
        }

        private void OnDestroy()
        {
            StopAllTimers();
        }

        public TimeModel CreateTimer(string id, int tickDuration, bool isRealtime = false)
        {
            var timeProgress = _progressService.RegionProgress.TimeOrderProgresses
                .GetOrCreateTimeProgress(id, tickDuration);

            TimeModel timeModel = new(timeProgress, tickDuration);
            return CreateTimeModel(id, tickDuration, timeModel, timeProgress);
        }

        private TimeModel CreateTimeModel(string id, int tickDuration, TimeModel timeModel,
            TimeProgress timeProgress)
        {
            if (TimeModels.ContainsKey(id))
            {
                TimeModels[id] = timeModel;
                TimeModels[id].TimeProgress = timeModel.TimeProgress;
            }
            else
            {
                timeModel = new(timeProgress, tickDuration);

                TimeModels.Add(id, timeModel);
            }
            
            Tick += timeModel.AboutTick;
            timeModel.Stopped += StoppedTimer;

            return TimeModels[id];
        }

        private void StoppedTimer(TimeModel timer)
        {
            Tick -= timer.AboutTick;
            timer.Stopped -= StoppedTimer;
            
            RemoveTimer(timer.TimeProgress.ID);
            StopTimer?.Invoke();
        }

        private void RemoveTimer(string id)
        {
            TimeModels.Remove(id);
        }

        private void StopAllTimers()
        {
            TimeModels.Clear();
        }
    }
}