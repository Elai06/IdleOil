using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Utils.Extensions
{
    /// <summary>
    ///     Defines an object that switches to a thread.
    /// </summary>
    [PublicAPI]
    public interface IThreadSwitcher : INotifyCompletion
    {
        bool IsCompleted { get; }

        IThreadSwitcher GetAwaiter();

        void GetResult();
    }

    /// <summary>
    ///     Switches to a particular thread.
    /// </summary>
    public static class ThreadSwitcher
    {
        /// <summary>
        ///     Switches to the Task thread.
        /// </summary>
        /// <returns></returns>
        public static IThreadSwitcher ResumeTaskAsync()
        {
            return new ThreadSwitcherTask();
        }

        /// <summary>
        ///     Switch to the Unity thread.
        /// </summary>
        /// <returns></returns>
        public static IThreadSwitcher ResumeUnityAsync()
        {
            return new ThreadSwitcherUnity();
        }
    }

    internal struct ThreadSwitcherTask : IThreadSwitcher
    {
        public IThreadSwitcher GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted => SynchronizationContext.Current == null;

        public void GetResult()
        {
        }

        public void OnCompleted([NotNull] Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            Task.Run(continuation);
        }
    }

    internal struct ThreadSwitcherUnity : IThreadSwitcher
    {
        public IThreadSwitcher GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted => SynchronizationContext.Current == UnityThread.Context;

        public void GetResult()
        {
        }

        public void OnCompleted([NotNull] Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            UnityThread.Context.Post(s => continuation(), null);
        }
    }

    internal static class UnityThread
    {
#pragma warning disable IDE0032 // Use auto property
        private static SynchronizationContext _context;
#pragma warning restore IDE0032 // Use auto property

        public static SynchronizationContext Context => _context;

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Capture()
        {
            _context = SynchronizationContext.Current;
        }
    }
}