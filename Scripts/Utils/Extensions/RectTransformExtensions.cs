using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Extensions
{
    public static class RectTransformExtensions
    {
        public static IEnumerator RebuildLayoutAfterFrames(this RectTransform rectTransform, int frames)
        {
            for (var i = 0; i < frames; i++)
            {
                yield return null;
            }
            
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}