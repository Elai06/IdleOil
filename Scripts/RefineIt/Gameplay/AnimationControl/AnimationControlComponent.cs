using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AnimationControl
{
    internal sealed class AnimationControlComponent<T> where T : Enum
    {
        private Animator _animator;

        private readonly Dictionary<T, int> _parametersHashes = new Dictionary<T, int>();
        
        private T _lastTriggerType;
        private bool _isTriggered;

        public void Initialize(Animator animator)
        {
            _animator = animator;

            foreach (T trigger in Enum.GetValues(typeof(T)))
            {
                _parametersHashes.Add(trigger, Animator.StringToHash(trigger.ToString()));
            }
        }

        public void SetTrigger(T parameter)
        {
            if (_isTriggered)
            {
                _animator.ResetTrigger(_parametersHashes[_lastTriggerType]);
            }
            
            _animator.SetTrigger(_parametersHashes[parameter]);
            
            _lastTriggerType = parameter;
            _isTriggered = true;
        }

        public void SetImmutableTrigger(T parameter)
        {
            _animator.SetTrigger(_parametersHashes[parameter]);
        }

        public void SetBool(T parameter, bool value)
        {
            _animator.SetBool(_parametersHashes[parameter], value);
        }
    }
}