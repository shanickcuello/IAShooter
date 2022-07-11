using Features.Drone.Scripts.Domain;
using Features.StateMachine;
using UnityEngine;

namespace Features.Drone.Scripts.FSM
{
    public class DroneDefendState<T> : States<T>
    {
        private readonly DroneController _droneController;
        private float _timeToStopDefending;
        
        public DroneDefendState(DroneController droneController)
        {
            _droneController = droneController;
        }

        public override void Awake()
        {
            _timeToStopDefending = 5;
            _droneController.Defending();
            Debug.Log("im defending");
        }

        public override void Execute()
        {
            _timeToStopDefending -= Time.deltaTime;
            _droneController.MoveOrStop();
            if (!(_timeToStopDefending <= 0)) return;
            _droneController.SearchForEnemy();
            _droneController.StopDefending();
            _droneController.ChangeState(EDroneStates.FollowLead);
        }

        public override void Exit()
        {
            _timeToStopDefending = 5;
            Debug.Log("im not defending");
        }
    }
}