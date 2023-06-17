using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MVVMLibrary.Infrastructure
{
    [CreateAssetMenu(fileName = "ViewsHolder", menuName = "Assets/ViewsHolder", order = 0)]
    public class ViewsHolder : ScriptableObject
    {
        public static string Path = "ViewsHolder";
        
        [AssetSelector(Paths = "Assets/_Project/Prefabs")]
        [SerializeField] private List<MonoBehaviour> _views;

        public List<MonoBehaviour> Views => _views;
    }
}