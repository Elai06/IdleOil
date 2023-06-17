using System;
using System.Collections.Generic;

namespace Infrastructure.PersistenceProgress
{
    [Serializable]
    public class TimeOrderProgresses
    {
        public List<TimeProgress> TimeProgresses = new();

        public TimeProgress GetOrCreateTimeProgress(string id, int time)
        {
            var timeProgress = TimeProgresses.Find(x => x.ID == id);

            if (timeProgress == null && time != 0)
            {
                TimeProgress progress = new(id, time);
                TimeProgresses.Add(progress);
                return progress;
            }

            return timeProgress;
        }

        public void RemoveTimeOrderProgress(string id)
        {
            var timeProgress = TimeProgresses.Find(x => x.ID == id);

            if (timeProgress != null)
            {
                TimeProgresses.Remove(timeProgress);
            }
        }
    }
}