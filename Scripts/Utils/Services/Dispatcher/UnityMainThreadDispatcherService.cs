using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Utils.Services
{
    public class UnityMainThreadDispatcherService : MonoBehaviour, IDispatcherService
    {
        private readonly Queue<Action> _waitQueue = new Queue<Action>();

        private int _lock;
        private bool _run;

        private void Update()
        {
            if (!_run)
            {
                return;
            }

            var executeQueue = CreateExecuteQueue();

            if (executeQueue == null)
            {
                return;
            }

            while (executeQueue.Count != 0)
            {
                var action = executeQueue.Dequeue();
                action();
            }
        }

        public void InvokeOnMainThread(Action action)
        {
            while (true)
            {
                if (0 == Interlocked.Exchange(ref _lock, 1))
                {
                    _waitQueue.Enqueue(action);
                    _run = true;
                    Interlocked.Exchange(ref _lock, 0);
                    break;
                }
            }
        }

        private Queue<Action> CreateExecuteQueue()
        {
            Queue<Action> executeQueue = null;
            if (0 == Interlocked.Exchange(ref _lock, 1))
            {
                executeQueue = new Queue<Action>(_waitQueue.Count);
                while (_waitQueue.Count != 0)
                {
                    var action = _waitQueue.Dequeue();
                    executeQueue.Enqueue(action);
                }

                _run = false;
                Interlocked.Exchange(ref _lock, 0);
            }

            return executeQueue;
        }
    }
}