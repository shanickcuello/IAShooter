using Features.StateMachine;
using UnityEngine;

namespace Features.Drone.Scripts.FSM
{
    public class DroneAttackState<T> : States<T>
    {
        public override void Awake()
        {
            Debug.Log("Estoy en attack");
        }
    }
}