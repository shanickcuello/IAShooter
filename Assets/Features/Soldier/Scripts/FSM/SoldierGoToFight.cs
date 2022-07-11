using System.Collections.Generic;
using Features.PathFinding;
using Features.Soldier.Scripts.Domain;
using Features.StateMachine;
using UnityEngine;

namespace Features.Soldier.Scripts.FSM
{
    public class SoldierGoToFight<T> : States<T>
    {
        private readonly SoldierController _soldierController;
        private Node _destinationNode;
        private Node _currentNode;
        private PathfindingManager _pathfindingManager;
        private SoldierView _soldierView;
        private List<Vector3> _path;


        public SoldierGoToFight(SoldierController soldierController,
            PathfindingManager pathfindingManager,
            SoldierView soldierView)
        {
            _soldierController = soldierController;
            _pathfindingManager = pathfindingManager;
            _soldierView = soldierView;
        }

        public override void Awake()
        {
            SetCurrentNodeFromPosition();
            SetFightPositionToGo();
            SetMoveAnimation();
            GetPath();
            Move();
        }

        private void Move()
        {
            _soldierController.MoveBy(_path, () => _soldierController.ChangeState(ESoldierStates.Patrol));
        }

        public override void Execute()
        {
            _soldierController.SearchForEnemy();
        }

        private void SetMoveAnimation()
        {
            _soldierView.SetAnimation(SoldierAnimations.Walking);
        }

        private void SetFightPositionToGo()
        {
            _destinationNode = _pathfindingManager.PositionToNode(
                _soldierController.GetFightPosition());
        }

        private void GetPath()
        {
            _path = _pathfindingManager.FindPath(_currentNode, _destinationNode);
        }
        
        private void SetCurrentNodeFromPosition()
        {
            _currentNode = _pathfindingManager.PositionToNode(_soldierController.transform.position);
        }

        public override void Exit()
        {
            _soldierController.StopMoving();
        }
    }
}