using Features.Drone.Scripts.Domain;
using Features.StateMachine;

namespace Features.Drone.Scripts.FSM
{
    public class DroneDeathState<T> : States<T>
    {
        private readonly DroneController _droneController;

        public DroneDeathState(DroneController droneController)
        {
            _droneController = droneController;
        }

        public override void Awake()
        {
            _droneController.Shutdown();
        }
    }
}