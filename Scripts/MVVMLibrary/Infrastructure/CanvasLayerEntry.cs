using System;
using MVVMLibrary.Enums;
using UnityEngine;

namespace MVVMLibrary.Infrastructure
{
    [Serializable]
    public class CanvasLayerEntry
    {
        public Layer Layer;
        public Canvas Canvas;
    }
}