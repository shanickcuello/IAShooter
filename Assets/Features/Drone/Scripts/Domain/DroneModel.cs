using UnityEngine;

namespace Features.Drone.Scripts.Domain
{
    public class DroneModel : MovementBehavior
    {
        Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        public override void Move(Vector3 dir)
        {
            dir.y = 0;
            _rb.velocity = dir * speed;
            transform.forward = Vector3.Lerp(transform.forward, dir, speedRot);
        }
    }
}