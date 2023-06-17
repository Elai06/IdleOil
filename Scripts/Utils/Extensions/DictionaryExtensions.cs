using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace  Utils.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns value by key or default value
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return dictionary.TryGetValue(key, out var result) ? result : defaultValue;
        }

        public static void AddToCollection<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key,
            TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(value);
            }
            else
            {
                dictionary.Add(key, new List<TValue>
                {
                    value
                });
            }
        }
        
        public static bool TryGetRandomKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, out TKey randomKey)
        {
            randomKey = default;
            var size = dictionary.Count;

            if (size == 0) return false;

            var keys = dictionary.Keys.ToList();
            randomKey = keys[Random.Range(0, size)];
            return true;
        }
        
        public static bool TryGetRandomValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, out TValue randomValue)
        {
            randomValue = default;
            var size = dictionary.Count;

            if (size == 0) return false;

            var values = dictionary.Values.ToList();
            randomValue = values[Random.Range(0, size)];
            return true;
        }

        public static bool TryAddPair<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            var result = !dictionary.TryGetValue(key, out var findValue);
            if(result)
            {
                dictionary.Add(key, value);
            }
            return result;
        }
    }
}
