using UnityEngine;

namespace Features.Weapons
{
    public interface IWeapon
    {
        void Fire(Transform transform);
        void ShutdownLight();
    }
}