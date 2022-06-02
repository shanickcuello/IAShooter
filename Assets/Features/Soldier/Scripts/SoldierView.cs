using UnityEngine;

namespace Features.Soldier.Scripts
{
    public class SoldierView : MonoBehaviour
    {
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetAnimation(SoldierAnimations soldierAnimation)
        {
            Debug.Log($"Transicion a {soldierAnimation.ToString()}");
            _animator.SetTrigger(soldierAnimation.ToString());
        }

        public void LookAt(Vector3 checkPoint)
        {
            var lookRotation = Quaternion.LookRotation(checkPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public enum SoldierAnimations
    {
        Idle,
        Walking,
        Shoot
    }
}
