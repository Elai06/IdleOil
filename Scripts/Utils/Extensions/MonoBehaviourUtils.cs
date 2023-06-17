using System;
using System.Collections;
using UnityEngine;

namespace  Utils.Extensions
{
    public static class MonoBehaviourUtils
    {
        /// <summary>
        /// Similar to MonoBehaviour.Invoke but takes Action as parameter
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public static void InvokeAction(this MonoBehaviour mb, Action action, float delay)
        {
            mb.StartCoroutine(InvokeRoutine(action, delay));
        }
 
        private static IEnumerator InvokeRoutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}
