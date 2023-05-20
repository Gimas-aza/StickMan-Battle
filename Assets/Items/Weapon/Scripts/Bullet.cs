using Assets.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Guns
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Collider _collider;
        private float _damage;
        private SpawnerBullet _spawnerBullet;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Transform hit = collision.transform;
            
            InteractionWithBullet(false);

            TryToDoDamage(hit);
            Invoke(nameof(Deactivate), 0.5f);
            Invoke(nameof(ReturnInSpawnerBullets), 0.5f);
        }

        public void SetDamageToBullet(float damage)
        {
            _damage = damage;
        }

        public void FireBullet(Vector3 bulletDirection, float shootForce)
        {
            _rigidbody.AddForce(bulletDirection * shootForce, ForceMode.Impulse);
        }

        public void SetComponentSpawnerBullet(SpawnerBullet spawner)
        {
            _spawnerBullet = spawner;
        }

        private void TryToDoDamage(Transform hit)
        {
            if (hit.TryGetComponent(out PlayerLogic playerLogic))
            {
                playerLogic.InflictDamage(_damage);
            }
        }

        private void Deactivate()
        {
            transform.gameObject.SetActive(false);
        }

        private void ReturnInSpawnerBullets()
        {
            transform.parent = _spawnerBullet.transform;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = _spawnerBullet.BulletDirection;

            InteractionWithBullet(true);
        }

        private void InteractionWithBullet(bool isEnabled)
        {
            _rigidbody.isKinematic = !isEnabled;
            _collider.enabled = isEnabled;
        }
    }
}