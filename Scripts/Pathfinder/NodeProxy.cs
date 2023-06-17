using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PathfinderNode
{
    public class NodeProxy : MonoBehaviour
    {
        [SerializeField] private List<NodeProxy> _neighbours;
        
        [SerializeField] private Node _data;

        public Node Data => _data;

        public List<NodeProxy> Neighbours => _neighbours;

        [Button("Save Data In Node")]
        public void SaveDataInNode()
        {
            _data.ChangeData(transform.position, _neighbours);
        }

        public void SetData(Node data)
        {
            _data = data;
            transform.position = data.GreedPosition;
        }
    }
}