using System;

namespace Gameplay.Workers
{
    [Serializable]
    public class WorkerProgress
    {
        public WorkerType WorkerType;
        public int Level;
        public bool IsBuy;

        public WorkerProgress(WorkerType workerType)
        {
            WorkerType = workerType;
        }
    }
}