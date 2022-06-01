using UnityEngine;

namespace Features.Soldier
{
    [CreateAssetMenu(fileName = "Soldier", menuName = "IAs", order = 0)]
    public class SoldierData : ScriptableObject
    {
        public float speedMovement;
    }
}