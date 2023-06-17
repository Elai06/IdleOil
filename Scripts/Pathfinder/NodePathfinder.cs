using System.Collections.Generic;

namespace PathfinderNode
{
    public class NodePathfinder : INavigationService
    {
        private List<Node> _path;
        private IGridGraph _graph;

        public void SetGraph(IGridGraph graph)
        {
            _graph = graph;
        }
        
        public List<Node> FindPath(Node startNode, Node goalNode)
        {
            _path = AStar.Search(_graph, startNode, goalNode);
            return _path;
        }

        // //TODO: для визуализации работы
        // private void OnDrawGizmosSelected()
        // {
        //     // Initialize a new GridGraph of a given width and height
        //     if (_map == null)
        //     {
        //         return;
        //     }
        //
        //     // Define the List of Vector3 to be considered walls
        //     _map.SetObstacles(_obstacles);
        //
        //     foreach (var node in _nodes)
        //     {
        //         Gizmos.DrawSphere(node.GreedPosition, 0.5f);
        //     }
        //
        //     // The Start node is BLUE
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawSphere(StartNode.GreedPosition, 0.5f);
        //
        //     // The obstacles are BLACK
        //     Gizmos.color = Color.black;
        //     foreach (var obstacle in _obstacles)
        //     {
        //         Gizmos.DrawSphere(obstacle.GreedPosition, 0.5f);
        //     }
        //
        //     foreach (Node n in _path)
        //     {
        //         Gizmos.color = n.GreedPosition == GoalNode.GreedPosition ? Color.red : Color.yellow;
        //
        //         Gizmos.DrawSphere(n.GreedPosition, 0.5f);
        //     }
        // }
    }
}