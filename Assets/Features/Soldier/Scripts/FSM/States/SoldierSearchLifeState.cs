using System.Collections.Generic;
using Features.PathFinding;
using UnityEngine;

namespace Features.Soldier.Scripts.FSM.States
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
            SaveCurrentPosition();
            MoveOverPath();
        }

        private void SaveCurrentPosition()
        {
            _soldierController.SetFightPosition();
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
    }
}