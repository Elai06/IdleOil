using System.Collections.Generic;
using UnityEngine;

namespace PathfinderNode
{
    [CreateAssetMenu(fileName = "NodeCollectionsPack", menuName = "Refine/Packs/NodeCollectionsPack", order = 0)]

    public class NodePack : ScriptableObject
    {
        [SerializeField] private List<Node> _nodes = new List<Node>();
        [SerializeField] private List<Node> _obstacles = new List<Node>();

        public List<Node> Nodes => _nodes;
        public List<Node> Obstacles => _obstacles;
    }
}