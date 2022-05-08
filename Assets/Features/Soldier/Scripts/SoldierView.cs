using System;
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

        public void TransitionTo(SoldierAnimations animation)
        {
            _animator.SetTrigger(animation.ToString());
        }
    }

    public enum SoldierAnimations
    {
        Idle,
        Walking
    }
}
