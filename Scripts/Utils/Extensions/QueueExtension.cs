using System;
using System.Collections.Generic;

namespace Utils.Extensions
{
    public static class QueueExtension
    {
        public static bool TryGetFirstElement<T>(this Queue<T> queue, out T result) where  T : class
        {
            try
            {
                result = queue.Dequeue();
                return true;
            }
            catch (Exception e)
            {
                result = null;
                Logger.LogError($"QueueExtension.TryGetFirstElement:\n {e}");
                return false;
            }
        }
    }
}
