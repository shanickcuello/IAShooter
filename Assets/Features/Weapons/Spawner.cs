using UnityEngine;

namespace Features.Weapons
{
    public class Spawner : MonoBehaviour
    {
        protected void Spawn(GameObject spawneable,Transform position, float speed, LayerMask layerMask)
        {
            var gameObject = Instantiate(spawneable, position.position, position.rotation);
            var spawned = gameObject.GetComponent<ISpawneable>();
            spawned.setSpeed(speed);    
            spawned.setLayerMask(layerMask);
        }
    }
}