using System;
using Features.Soldier.Scripts.FSM;
using Features.Soldier.Scripts.FSM.States;
using UnityEngine;

namespace Features.Soldier.Scripts
{ public class SoldierController : MonoBehaviour
    {
        private SoldierView _soldierView;
        private SoldierIdleState<ESoldierStates> _soldierIdleState;
        private SoldierPatrolState<ESoldierStates> _soldierPatrolState;
        private FSM<ESoldierStates> _fsm;

        private void Awake()
        {
            _soldierView = GetComponent<SoldierView>();
            _soldierIdleState = new SoldierIdleState<ESoldierStates>(this, _soldierView);
            _soldierPatrolState = new SoldierPatrolState<ESoldierStates>(this, _soldierView);

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
    }
}