using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Zenject;

namespace PathfinderNode
{
    public class GraphManager : MonoBehaviour
    {
        [SerializeField] private NodeProxy _nodeProxyPrefab;
        [SerializeField] private NodePack _nodePack;
        [SerializeField] private bool _isDebugMode;

        private Dictionary<Node, NodeProxy> _proxyByNode = new Dictionary<Node, NodeProxy>();
        private List<Node> _nodes = new List<Node>();
        private List<Node> _obstacles = new List<Node>();
        private IGridGraph _graph;
        private INavigationService _navigationService;
        private Camera _camera;

        public List<Node> Nodes => _nodes;

        [Inject]
        private void Construct(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private void Start()
        {
            _camera = Camera.main;
            LoadGridData();
            
            if (_isDebugMode)
            {
                _proxyByNode.Clear();
                SpawnProxyNodes();
                SpawnProxyObstacles();
                SetProxyNeighbours();
            }

            _graph = new CustomGraph(Nodes, _obstacles);
            _navigationService?.SetGraph(_graph);
        }

        private void SetProxyNeighbours()
        {
            foreach (var nodePair in _proxyByNode)
            {
                foreach (var neighbour in nodePair.Key.Neighbours)
                {
                    var b = _proxyByNode.Keys.FirstOrDefault(x => x.GreedPosition == neighbour.GreedPosition);

                    if (_proxyByNode.TryGetValue(b, out var foundProxy))
                    {
                        if (nodePair.Value.Data == foundProxy.Data)
                        {
                            continue;
                        }

                        nodePair.Value.Neighbours.Add(foundProxy);
                    }
                    else
                    {
                        var a = _proxyByNode.Keys.FirstOrDefault(x => x.GreedPosition == neighbour.GreedPosition);
                        var gch = GCHandle.Alloc(neighbour, GCHandleType.WeakTrackResurrection);
                        var address = GCHandle.ToIntPtr(gch).ToInt32();
                        var gch2 = GCHandle.Alloc(a, GCHandleType.WeakTrackResurrection);
                        var address2 = GCHandle.ToIntPtr(gch2).ToInt32();
                        this.Log($"Couldn't find {nodePair.Key}'s neighbour {neighbour}");
                    }
                }
            }
        }

        private void SpawnProxyObstacles()
        {
            foreach (var obstacle in _obstacles)
            {
                var proxyObstacle = Instantiate(_nodeProxyPrefab, obstacle.GreedPosition, Quaternion.identity);
                proxyObstacle.SetData(obstacle);
                _proxyByNode.Add(obstacle, proxyObstacle);
            }
        }

        private void SpawnProxyNodes()
        {
            foreach (var node in _nodes)
            {
                var proxyNode = Instantiate(_nodeProxyPrefab, node.GreedPosition, Quaternion.identity);
                proxyNode.SetData(node);
                _proxyByNode.Add(node, proxyNode);
            }
        }

        [Button("Create New Node")]
        private void CreateNewNode()
        {
            if (!_isDebugMode)
            {
                return;
            }

            AddNode();
        }

        private void AddNode()
        {
            var node = new Node(Vector3.zero);
            var neighbours = FindNearNeighbours(node);

            node.SetNeighbours(neighbours);
            
            foreach (var neighbour in neighbours)
            {
                neighbour.AddNeighbours(node);
            }
            
            var proxy = Instantiate(_nodeProxyPrefab, node.GreedPosition, Quaternion.identity);
            proxy.SetData(node);
            _proxyByNode.Add(node, proxy);
            Nodes.Add(node);
            
            _graph = new CustomGraph(Nodes, _obstacles);
            _navigationService?.SetGraph(_graph);
        }

        private List<Node> FindNearNeighbours(Node newNode)
        {
            return Nodes.FindAll(node => Vector3.Distance(newNode.GreedPosition, node.GreedPosition) < 3);
        }

        public void AddObstacle(Node node)
        {
            _obstacles.Add(node);
            _graph = new CustomGraph(Nodes, _obstacles);
            _navigationService?.SetGraph(_graph);
        }

        private void OnDrawGizmos()
        {
            foreach (var node in _nodes)
            {
                foreach (var neighbour in node.Neighbours)
                {
                    Gizmos.DrawLine(node.GreedPosition, neighbour.GreedPosition);
                }
            }
        }

        [Button("Save nodes")]
        private void SaveNodes()
        {
            if (!_isDebugMode)
            {
                return;
            }

            foreach (var nodePair in _proxyByNode)
            {
                nodePair.Value.SaveDataInNode();
            }
        }

        [Button("Load nodes")]
        private void LoadGridData()
        {
            _nodes = _nodePack.Nodes;
            _obstacles = _nodePack.Obstacles;
        }
    }
}