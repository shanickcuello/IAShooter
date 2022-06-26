using System.Collections;
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
            if(_animator == null)
                _animator = GetComponent<Animator>();
            _animator.SetTrigger(soldierAnimation.ToString());
        }

        public void LookAt(Transform target)
        {
            var lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            var targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            transform.rotation = new Quaternion(0, targetRotation.y, 0, targetRotation.w);
        }
        
        public void LookAt(Vector3 target)
        {
            var lookRotation = Quaternion.LookRotation(target - transform.position);
            var targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            transform.rotation = new Quaternion(0, targetRotation.y, 0, targetRotation.w);
        }

        public void Death()
        {
            SetAnimation(SoldierAnimations.Death);
        }
    }

    public enum SoldierAnimations
    {
        Idle,
        Walking,
        Shoot,
        Death
    }
}