using System;
using UnityEngine;

namespace Features.LifeSystem
{
    public class Life 
    {
        private int _maxLife;
        [SerializeField] private int _lifeAmount;
        private readonly Action _onDeath;

        public Life(int maxLife, Action onDeath)
        {
            _lifeAmount = maxLife;
            _onDeath = onDeath;
        }


        public void DecreaseLife(int damagePower)
        {
            _lifeAmount -= damagePower;
            Debug.Log($"mi vida es: {_lifeAmount}");
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (_lifeAmount <= 0)
            {
                _onDeath();
            }
        }

        public void AddLife(int life) => Mathf.Min(_maxLife, _lifeAmount += life);
    }
}