using System.Globalization;
using System.Linq;

namespace Utils.Extensions
{
    public static class StringExtensions
    {
        private static readonly TextInfo MyTi = new CultureInfo("en-US",false).TextInfo;
        
        public static string ToTitleCase(this string text)
        {
            return MyTi.ToTitleCase(text);
        }

        public static int NIndexOf(this string text, char value, int nOccurence)
        {
            var result = text.Select((c, i) => new { Char = c, Index = i })
                .Where(item => item.Char == value)
                .Skip(nOccurence - 1)
                .FirstOrDefault();

            var index = result?.Index ?? -1;
            //Debug.Log($"StringExtension.NIndexOf index {index} of '{value}' in \"{text}\"");
            
            return index;
        }
    }
}