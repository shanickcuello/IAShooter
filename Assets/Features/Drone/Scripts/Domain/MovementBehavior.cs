using UnityEngine;

namespace Features.Drone.Scripts.Domain
{
    public class MovementBehavior : MonoBehaviour
    {
        public float speed;
        public float speedRot;

        public virtual void Move(Vector3 dir)
        {
            dir.y = 0;
            transform.position += Time.deltaTime * dir * speed;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speedRot);
        }
    }
}