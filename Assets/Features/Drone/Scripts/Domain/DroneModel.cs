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
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speedRot);
        }
    }
}