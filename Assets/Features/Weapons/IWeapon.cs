using UnityEngine;

namespace Features.Soldier.Scripts.Weapons
{
    public interface IWeapon
    {
        void Fire(Transform transform);
    }
}