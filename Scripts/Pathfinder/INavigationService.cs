using System.Collections.Generic;

namespace PathfinderNode
{
    public interface INavigationService
    {
        List<Node> FindPath(Node startNode, Node goalNode);
        void SetGraph(IGridGraph graph);
    }
}