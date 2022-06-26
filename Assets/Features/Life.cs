
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Features
{
    public class Life
    {
        private int _lifeAmount;
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
    }
}