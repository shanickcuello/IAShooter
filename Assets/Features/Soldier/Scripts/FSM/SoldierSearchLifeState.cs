using System.Collections.Generic;
using Features.PathFinding;
using Features.Soldier.Scripts.Domain;
using Features.StateMachine;
using UnityEngine;

namespace Features.Soldier.Scripts.FSM
{
    public class SoldierSearchLifeState<T> : States<T>
    {
        private readonly SoldierController _soldierController;
        private Node _destinationNode;
        private Node _currentNode;
        private List<Vector3> _path;
        private PathfindingManager _pathfindingManager;
        private readonly SoldierView _soldierView;
        private Vector3 _fightPosition;
        private float _timeSearchingLife;

        public SoldierSearchLifeState(SoldierController soldierController, 
            PathfindingManager pathfindingManager,
            SoldierView soldierView)
        {
            _soldierView = soldierView;
            _pathfindingManager = pathfindingManager;
            _soldierController = soldierController;
        }

        public override void Awake()
        {
            SetTimeSeachingLife();
            SaveCurrentPosition();
            MoveOverPath();
        }

        private void SetTimeSeachingLife()
        {
            _timeSearchingLife = 5;
        }

        private void SaveCurrentPosition()
        {
            DontSearchLife();
            _soldierController.SetFightPosition();
        }

        private void DontSearchLife()
        {
            _timeSearchingLife -= Time.deltaTime;
            if (_timeSearchingLife <= 0)
                _soldierController.ChangeState(ESoldierStates.Idle);
        }

        private void MoveOverPath()
        {
            SetCurrentNodeFromPosition();
            SetLifePowerUpToGo();
            CreatePathFromDestinationNode();
            Move();
            SetMoveAnimation();
        }

        private void SetMoveAnimation()
        {
            _soldierView.SetAnimation(SoldierAnimations.Walking);
        }

        private void Move()
        {
            _soldierController.MoveBy(_path, () => _soldierController.ChangeState(ESoldierStates.GoToFight));
        }

        private void CreatePathFromDestinationNode()
        {
            _path = _pathfindingManager.FindPath(_currentNode, _destinationNode);
        }
        
        private void SetLifePowerUpToGo()
        {
            _destinationNode = _pathfindingManager.
                PositionToNode(
                    _soldierController.
                        GetClosestLifePowerUp());
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