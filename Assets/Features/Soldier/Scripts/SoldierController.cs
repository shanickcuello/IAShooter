using System;
using System.Collections;
using System.Collections.Generic;
using Features.PathFinding;
using Features.Soldier.Scripts.FSM;
using Features.Soldier.Scripts.FSM.States;
using UnityEngine;

namespace Features.Soldier.Scripts
{
    public class SoldierController : MonoBehaviour
    {
        [SerializeField] private float soldierSpeed;
        [SerializeField] PathfindingManager pathfindingManager;
        private SoldierView _soldierView;
        private SoldierIdleState<ESoldierStates> _soldierIdleState;
        private SoldierPatrolState<ESoldierStates> _soldierPatrolState;
        private FSM<ESoldierStates> _fsm;

        protected void Awake()
        {
            _soldierView = GetComponent<SoldierView>();
            _soldierIdleState = new SoldierIdleState<ESoldierStates>(this, _soldierView);
            _soldierPatrolState = new SoldierPatrolState<ESoldierStates>(this, _soldierView, pathfindingManager);

            _soldierIdleState.AddTransitionState(ESoldierStates.Patrol, _soldierPatrolState);

            _fsm = new FSM<ESoldierStates>(_soldierIdleState);
        }
        

        private void Update()
        {
            _fsm.OnUpdate();
        }

        public void ChangeState(ESoldierStates state)
        {
            _fsm.Transition(state);
        }

        public void Move(List<Vector3> path)
        {
            StartCoroutine(FollowPath(path));
        }

        private IEnumerator FollowPath(List<Vector3> path)
        {
            foreach (var pos in path)
            {
                Vector3 checkPoint = pos;
                checkPoint.y = transform.position.y;

                while (transform.position != checkPoint)
                {
                    transform.position =
                        Vector3.MoveTowards(transform.position, checkPoint, soldierSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }

        public void SearchForEnemy()
        {
            
        }
    }
}