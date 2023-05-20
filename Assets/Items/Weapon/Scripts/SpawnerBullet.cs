using System.Collections.Generic;
using UnityEngine;

namespace Assets.Guns
{
    public class SpawnerBullet : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _numberOfBullets;
        [SerializeField] private float _shootForce;
        [SerializeField] private GameObject _containerBullets;

        private readonly List<Bullet> _bullets = new();
        private int _currentIndexBulletsForFired;

        public Vector3 BulletDirection { get; private set; } = new Vector3(-90f, 0, 0);

        private void Start()
        {
            MakeBullets();
        }

        public void ShotBullet(float bulletDamage)
        {
            if (_currentIndexBulletsForFired >= _bullets.Count)
                _currentIndexBulletsForFired = 0;

            Bullet currentBullet = _bullets[_currentIndexBulletsForFired];

            currentBullet.transform.parent = _containerBullets.transform;
            currentBullet.gameObject.SetActive(true);

            currentBullet.SetDamageToBullet(bulletDamage);
            currentBullet.FireBullet(-transform.forward, _shootForce);

            _currentIndexBulletsForFired++;
        }

        private void MakeBullets()
        {
            Bullet bullet;

            for (int i = 0; i < _numberOfBullets; i++)
            {
                bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity, transform);
                bullet.transform.localEulerAngles = BulletDirection;

                bullet.SetComponentSpawnerBullet(this);

                bullet.gameObject.SetActive(false);

                _bullets.Add(bullet);
            }
        }
    }
}