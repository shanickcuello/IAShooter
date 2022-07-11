using Features.Drone.Scripts.Domain;
using Features.StateMachine;

namespace Features.Drone.Scripts.FSM
{
    public class DroneFollowLeadState<T> : States<T>
    {
        private readonly DroneController _droneController;

        public DroneFollowLeadState(DroneController droneController)
        {
            _droneController = droneController;
        }

        public override void Awake()
        {
            _droneController.FollowLead();
        }

        public override void Execute()
        {
            _droneController.MoveOrStop();
            _droneController.SearchForEnemy();
        }
    }
}
