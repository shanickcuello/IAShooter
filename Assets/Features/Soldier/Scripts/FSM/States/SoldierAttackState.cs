using TMPro;
using UnityEngine;

namespace Features.Soldier.Scripts.FSM.States
{
    public class SoldierAttackState<T> : States<T>
    {
        private SoldierController _soldierController;
        private SoldierView _soldierView;
        public SoldierAttackState(SoldierController controller, SoldierView view)
        {
            _soldierController = controller;
            _soldierView = view;
        }

        public override void Awake()
        {
            _soldierController.StopMoving();
            _soldierController.ShootIntermittent();
        }

        public override void Execute()
        {
            _soldierView.LookAt(_soldierController.target);
        }
    }
}