using System.Collections.Generic;
using UnityEngine;

namespace PathfinderNode
{
    public interface IGridGraph
    {
        public List<Node> Grid { get; }
        public List<Node> Obstacles { get; }

        List<Node> GetPassableNeighbours(Node currentNode);
        float Cost(Node current, Node next);
        void SetObstacles(List<Node> obstacles);
    }
}