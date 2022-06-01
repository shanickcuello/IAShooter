using UnityEngine;

namespace Features.Soldier.Scripts.FSM.States
{
    public class SoldierIdleState<T> : States<T>
    {
        private readonly SoldierController _soldierController;
        private readonly SoldierView _soldierView;
        private float timeToChangeState = 5;
        private float _currentTimeToChangeState;
        
        public SoldierIdleState(SoldierController soldierController, SoldierView soldierView)
        {
            _soldierController = soldierController;
            _soldierView = soldierView;
        }


        public override void Awake()
        {
            _soldierView.SetAnimation(SoldierAnimations.Idle);
            Debug.Log("Awake de " + SoldierAnimations.Idle);
            _currentTimeToChangeState = timeToChangeState;
        }

        public override void Execute()
        {
            _currentTimeToChangeState-= Time.deltaTime;
            if (_currentTimeToChangeState <= 0)
            {
                _soldierController.ChangeState(ESoldierStates.Patrol);
            }
        }

        public override void Exit()
        {
            _currentTimeToChangeState = timeToChangeState;
        }
    }
}