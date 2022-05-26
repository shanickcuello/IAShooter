using System.Collections.Generic;
using Features.PathFinding;
using UnityEngine;

namespace Features.Soldier.Scripts.FSM.States
{
    public class SoldierPatrolState<T> : States<T>
    {
        private PathfindingManager _pathfindingManager;
        private SoldierController _soldierController;
        private SoldierView _soldierView;
        private Node _destinationNode;
        private Node _currentNode;
        private List<Vector3> _path;

        public SoldierPatrolState(SoldierController soldierController, SoldierView soldierView,
            PathfindingManager pathfindingManager)
        {
            _soldierController = soldierController;
            _soldierView = soldierView;
            _pathfindingManager = pathfindingManager;
            _soldierView.SetAnimation(SoldierAnimations.Walking);
        }

        public override void Awake()
        {
            Debug.Log("Moving");
            SetCurrentNodeFromPosition();
            SetRandomNodeToGo();
            CreatePathFromDestinationNode();
            MoveByPath();
        }
        
        public override void Execute()
        {
            _soldierController.SearchForEnemy();
        }

        private void MoveByPath()
        {
            _soldierController.Move(_path);
        }

        private void CreatePathFromDestinationNode()
        {
            _path = _pathfindingManager.FindPath(_currentNode, _destinationNode);
        }

        private void SetRandomNodeToGo()
        {
            _destinationNode = _pathfindingManager.GetRandomNode();
        }

        private void SetCurrentNodeFromPosition()
        {
            _currentNode = _pathfindingManager.PositionToNode(_soldierController.transform.position);
        }

      
    }
}