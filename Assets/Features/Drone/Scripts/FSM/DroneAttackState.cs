using Features.Drone.Scripts.Domain;
using Features.StateMachine;
using UnityEngine;

namespace Features.Drone.Scripts.FSM
{
    public class DroneAttackState<T> : States<T>
    {
        private readonly DroneController _droneController;

        public DroneAttackState(DroneController droneController)
        {
            _droneController = droneController;
        }
        
        public override void Awake()
        {
            Debug.Log("Estoy en attack");
            _droneController.FollowEnemy();
            _droneController.ShootIntermittent();
        }

        public override void Execute()
        {
            _droneController.MoveOrStop();
            _droneController.CheckEnemySteering();
        }
    }
}