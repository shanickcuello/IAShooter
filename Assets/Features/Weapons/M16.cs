using Features.Soldier.Scripts;
using Features.Soldier.Scripts.Weapons;
using UnityEngine;

namespace Features.Weapons
{
    public class M16 : Spawner, IWeapon
    {
        [SerializeField] Transform _bulletSpawnPosition;
        [SerializeField] private float _speedMovement;
        [SerializeField] private GameObject _bullet;
        public void Fire(Transform transform)
        {
            Spawn(_bullet, _bulletSpawnPosition, _speedMovement, LayerMask.NameToLayer("Blue"));
        }
    }
}
