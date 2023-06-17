using UnityEngine;
using UnityEngine.UI;

namespace Utils.Extensions
{
    public static class ImageExtensions
    {
        public static void SetAlpha(this Image image, float a)
        {
            image.color = new Color(image.color.r, image.color.g,
                image.color.b, a);
        }
    }
}