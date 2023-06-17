using System;
using UnityEngine;

namespace Gameplay.Settings
{
    [Serializable]
    public struct AudioSFX
    {
        public ClickClipId clickClipGuid;
        public AudioClip Clip;
    }
}