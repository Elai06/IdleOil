using System.Collections.Generic;
using UnityEngine;

namespace PathfinderNode
{
    public class CustomGraph : IGridGraph
    {
        public List<Node> Grid { get; private set; }
        public List<Node> Obstacles { get; private set; }

        public CustomGraph(List<Node> nodes, List<Node> obstacles)
        {
            Grid = nodes;
            Obstacles = obstacles;
        }

        public List<Node> GetPassableNeighbours(Node currentNode)
        {
            return currentNode.Neighbours;
        }

        public float Cost(Node current, Node next)
        {
            var distance = Vector3.Distance(current.GreedPosition, next.GreedPosition);
            if (Obstacles.Contains(next))
            {
                distance = float.MaxValue;
            }

            return distance;
        }

        public void SetObstacles(List<Node> obstacles)
        {
            Obstacles = obstacles;
        }
    }
}