using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Gameplay.Settings
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Data/AudioData")]
    public class AudioData : ScriptableObject
    {
        public AudioMixer Mixer;
        public List<AudioSFX> AudioClips;
    }
}