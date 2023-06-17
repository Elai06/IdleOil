using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace PathfinderNode
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private GraphManager _graphManager;
        [SerializeField] private float _speed;

        //test
        [SerializeField] private int _start;
        [SerializeField] private int _end;

        private List<Node> _path = new List<Node>();
        private INavigationService _navigationService;
        private Sequence move;

        [Inject]
        private void Construct(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [ContextMenu("Find Path")]
        public void FindPath()
        {
            _path = _navigationService.FindPath(_graphManager.Nodes[_start],
                _graphManager.Nodes[_end]);

            StartMove();
        }

        private void StartMove()
        {
            move?.Kill();
            move = DOTween.Sequence();
            transform.position = _path[0].GreedPosition;
            var currentPosition = transform.position;
            move.AppendInterval(1);
            
            for (var i = 1; i < _path.Count; i++)
            {
                var duration = Vector3.Distance(currentPosition, _path[i].GreedPosition) / _speed;
                move.Append(transform.DOMove(_path[i].GreedPosition, duration).SetEase(Ease.Linear));
                move.Join(transform.DOLookAt(_path[i].GreedPosition, 0.5f));
                currentPosition = _path[i].GreedPosition;
            }
            
            //колбек для теста
            move.AppendCallback(() => { SetNewGoal(); });
        }

        private void SetNewGoal()
        {
            _start = _end;
            _end = Random.Range(0, _graphManager.Nodes.Count);
            FindPath();
        }

        public void SetPath(List<Node> path)
        {
            _path = path;
        }

        //TODO: для визуализации работы
        private void OnDrawGizmosSelected()
        {
            if (_path.Count == 0)
            {
                return;
            }

            Gizmos.color = Color.red;
            for (var index = 0; index < _path.Count - 1; index++)
            {
                var c = _path[index];
                var n = _path[index + 1];
                Gizmos.DrawLine(c.GreedPosition, n.GreedPosition);
            }
        }
    }
}