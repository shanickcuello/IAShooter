using UnityEngine;

namespace Features.Weapons
{
    public interface IWeapon
    {
        void Fire(int layer);
        void ShutdownLight();
    }
}