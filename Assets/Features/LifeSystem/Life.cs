using System;
using UnityEngine;

namespace Features.LifeSystem
{
    public class Life 
    {
        private int _maxLife;
        public int LifeAmount;
        private readonly Action _onDeath;

        public Life(int maxLife, Action onDeath)
        {
            LifeAmount = maxLife;
            _onDeath = onDeath;
        }


        public void DecreaseLife(int damagePower)
        {
            LifeAmount -= damagePower;
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (LifeAmount <= 0)
            {
                _onDeath();
            }
        }

        public void AddLife(int life) => Mathf.Min(_maxLife, LifeAmount += life);
    }
}