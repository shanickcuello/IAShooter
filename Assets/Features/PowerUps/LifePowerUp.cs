using System.Collections;
using UnityEngine;

namespace Features.PowerUps
{
    public class LifePowerUp : MonoBehaviour
    {
        [SerializeField] private int lifePower;

        public int Consume()
        {
            StartCoroutine(DestroyAfter(1));
            return lifePower;
        }

        private IEnumerator DestroyAfter(int time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}
