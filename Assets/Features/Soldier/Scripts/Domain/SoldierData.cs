using UnityEngine;

namespace Features.Soldier.Scripts.Domain
{
    [CreateAssetMenu(fileName = "Soldier", menuName = "IAs", order = 0)]
    public class SoldierData : ScriptableObject
    {
        public float speedMovement;
        public float timeBetweenShoots;
        public int life;
    }
}