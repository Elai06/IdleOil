using DG.Tweening;
using TMPro;

namespace Utils.Extensions
{
    public static class TextMeshProUGUIExtension
    {
        public static Tweener DOCounter(this TextMeshProUGUI text, int from, int to, float duration)
        {
            text.DOKill();
            return DOVirtual.Int(from, to, duration, x => text.text = x.ToSpaceBetweenChars());
        }
    }
}