using System.Collections.Generic;

namespace Utils.Extensions
{
    public static class ListExtension
    {
        private static readonly System.Random Random = new System.Random();

        /// <summary>
        /// Return random element from list
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T GetRandom<T>(this List<T> list, int exceptIndex, out int resultIndex)
        {
            resultIndex = UnityEngine.Random.Range(0, list.Count);
            if (resultIndex == exceptIndex && list.Count > 1)
            {
                resultIndex = (resultIndex + 1) % list.Count;
            }

            return list[resultIndex];
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}