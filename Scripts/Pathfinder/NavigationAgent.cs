using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathfinderNode
{
    public class NavigationAgent : MonoBehaviour
    {
        [SerializeField] private List<Vector3> _path;

        public void SetPath(List<Vector3> path)
        {
            _path = path;
        }

        private void Update()
        {
            throw new NotImplementedException();
        }
    }
}