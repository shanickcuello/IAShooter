using System;
using System.Collections;
using Features.Weapons.Bullets.Code;
using UnityEngine;

namespace Features.Weapons
{
    public class Bullet : MonoBehaviour, ISpawneable
    {
        [SerializeField] private int damagePower;
        LayerMask _layerMask;
        float _speedMovment;

        public Bullet(LayerMask layerMask, float speedMovement)
        {
            _speedMovment = speedMovement;
            _layerMask = layerMask;
        }

        private void Start()
        {
            StartCoroutine(DestroyMySelf());
        }

        private IEnumerator DestroyMySelf()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }

        private void Update()
        {
            MoveForward();
        }

        private void MoveForward()
        {
            transform.position += transform.forward * Time.deltaTime * _speedMovment;
        }

        public void setSpeed(float speed)
        {
            _speedMovment = speed;
        }

        public void setLayerMask(LayerMask layerMask)
        {
            _layerMask = layerMask;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == _layerMask.value) return;
            if (collision.gameObject.GetComponent<IDamageable>() != null)
            {
                collision.gameObject.GetComponent<IDamageable>().MakeDamage(transform.position, damagePower);
            }
            Destroy(gameObject);
        }
    }
}