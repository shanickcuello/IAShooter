using Features.Soldier.Scripts.Weapons;
using UnityEngine;

namespace Features.Weapons
{
    public class M16 : MonoBehaviour, IWeapon
    {
        public void Fire(Transform transform)
        {
            Debug.Log("FIRE");
        }
    }
}
