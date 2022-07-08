using System.Collections.Generic;
using Features.PathFinding;
using Features.Soldier.Scripts.Domain;
using Features.StateMachine;
using UnityEngine;

namespace Features.Soldier.Scripts.FSM
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
        }

        public override void Awake()
        {
            MoveOverPath();
        }

        public override void Execute()
        {
            _soldierController.SearchForEnemy();
            _soldierController.SearchForLife();
        }

        private void MoveOverPath()
        {
            SetCurrentNodeFromPosition();
            SetRandomNodeToGo();
            CreatePathFromDestinationNode();
            _soldierController.MoveBy(_path, ChangeToIdle);
            _soldierView.SetAnimation(SoldierAnimations.Walking);
        }

        private void ChangeToIdle()
        {
            _soldierController.ChangeState(ESoldierStates.Idle);
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