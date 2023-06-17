using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathfinderNode
{
    [Serializable]
    public class Node : IComparable<Node>
    {
        [SerializeField] private List<Node> _neighbours = new List<Node>();
        [SerializeField] private float _priority;
        [SerializeField] private Vector3 _greedPosition;
        
        public float Priority => _priority;
        public Vector3 GreedPosition => _greedPosition;
        public List<Node> Neighbours => _neighbours;

        public Node(Vector3 position)
        {
            _greedPosition = position;
        }

        public void SetNeighbours(List<Node> neighbours)
        {
            _neighbours = neighbours;
        }
        
        public void AddNeighbours(Node neighbour)
        {
            _neighbours.Add(neighbour);
        }

        public int CompareTo(Node other)
        {
            if (this.Priority < other.Priority) return -1;
            else if (this.Priority > other.Priority) return 1;
            else return 0;
        }

        public void SetPriority(float priority)
        {
            _priority = priority;
        }

        public void ChangeData(Vector3 position, List<NodeProxy> neighbours)
        {
            _greedPosition = position;
            _neighbours.Clear();
            
            foreach (var neighbour in neighbours)
            {
                _neighbours.Add(neighbour.Data);
            }
        }
    }
}