using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Priority_Queue;
using UnityEngine;

namespace PathfinderNode
{
    public static class AStar
    {
        /// <summary>
        /// Returns the best path as a List of Nodes
        /// </summary>
        public static List<Node> Search(IGridGraph grid, Node start, Node goal)
        {
            var connections = new Dictionary<Node, Node>();
            var transitionCosts = new Dictionary<Node, float>();

            var path = new List<Node>();

            var frontier = new SimplePriorityQueue<Node>();
            frontier.Enqueue(start, 0);

            connections.Add(start, start);
            transitionCosts.Add(start, 0);

            Node current = null;
            while (frontier.Count > 0)
            {
                current = frontier.Dequeue();
                if (current.GreedPosition == goal.GreedPosition) break; // Early exit

                foreach (var next in grid.GetPassableNeighbours(current))
                {
                    var newCost = transitionCosts[current] + grid.Cost(current, next);
                    if (!transitionCosts.ContainsKey(next) || newCost < transitionCosts[next])
                    {
                        transitionCosts[next] = newCost;
                        connections[next] = current;
                        var priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        next.SetPriority(newCost);
                    }
                }
            }

            while (current != start)
            {
                path.Add(current);
                current = connections[current];
            }
            
            path.Add(start);
            path.Reverse();

            return path;
        }

        private static float Heuristic(Node a, Node b)
        {
            return Vector3.Distance(a.GreedPosition, b.GreedPosition);
            ;
        }
    }
}