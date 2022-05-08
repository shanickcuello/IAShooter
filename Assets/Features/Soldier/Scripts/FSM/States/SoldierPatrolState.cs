using UnityEngine;

namespace Features.Soldier.Scripts.FSM.States
{
    public class SoldierPatrolState<T> : States<T>
    {
        private SoldierController _soldierController;
        private SoldierView _soldierView;

        public SoldierPatrolState(SoldierController soldierController, SoldierView soldierView)
        {
            _soldierController = soldierController;
            _soldierView = soldierView;
        }

        public override void Awake()
        {
            Debug.Log("Init patrol");
        }

        public override void Execute()
        {
            Debug.Log("exe patrol");
        }
    }
}