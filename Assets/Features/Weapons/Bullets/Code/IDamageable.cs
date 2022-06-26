using UnityEngine;

namespace Features.Weapons.Bullets.Code
{
    public interface IDamageable
    {
        void MakeDamage(Vector3 transformPosition, int damagePower);
    }
}