using MVVMLibrary.Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Windows
{
    [CreateAssetMenu(fileName = "Window", menuName = "Data/Window")]
    public class WindowStaticData : ScriptableObject
    {
        public WindowType Type;
        public Layer Layer;
        public AssetReferenceGameObject Window;
    }
}