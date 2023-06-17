namespace Utils.Extensions
{
    public static class ArrayExtensions
    {
        public static T GetRandom<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                Logger.LogError($"Array is empty");
                return default;
            }
            
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        
        public static T GetRandom<T>(this T[] array, out int index)
        {
            if (array == null || array.Length == 0)
            {
                Logger.LogError($"Array is empty");
            }

            index = UnityEngine.Random.Range(0, array.Length);
            return array[index];
        }
    }
}