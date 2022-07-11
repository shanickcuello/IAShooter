using Features.Soldier.Scripts;
using UnityEngine;

namespace Features.Weapons
{
    public class M16 : Spawner, IWeapon
    {
        [SerializeField] Transform _bulletSpawnPosition;
        [SerializeField] private float _speedMovement;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Light light;
        public void Fire(int layer)
        {
            Spawn(_bullet, _bulletSpawnPosition, _speedMovement, layer);
        }

        public void ShutdownLight()
        {
            light.enabled = false;
        }
    }
}
