using System.Collections.Generic;
using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class BulletSystem : MonoBehaviour, IAwake, IFixedUpdate, IPause, IResume
    {
        [SerializeField]
        private int _initialCount = 50;

        [SerializeField] private Transform _container;
        [SerializeField] private Bullet _prefab;
        [SerializeField] private Transform _worldTransform;
        [SerializeField] private LevelBounds _levelBounds;
        [SerializeField] private GameFlowManager _gameFlowManager;

        private bool _isPaused;

        private BulletPool _bulletPool;
        private readonly List<Bullet> _cache = new();

        public void AwakeObj()
        {
            _bulletPool = new(_initialCount, _prefab, _container, _worldTransform, OnBulletCollision, _gameFlowManager);
        }

        public void FixedUpdateObj()
        {
            if (_isPaused)
                return;

            _cache.Clear();
            _cache.AddRange(_bulletPool.GetBullets());

            for (int i = 0, count = _cache.Count; i < count; i++)
            {
                var bullet = _cache[i];
                if (!_levelBounds.InBounds(bullet.transform.position))
                {
                    _bulletPool.DespawnBullet(bullet);
                }
            }
        }

        public void FlyBulletByArgs(Args args)
        {
            if (_isPaused)
            {
                return;
            }

            _bulletPool.SpawnBullet(args);
        }

        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            DealDamage(bullet, collision.gameObject);
            _bulletPool.DespawnBullet(bullet);
        }

        private void DealDamage(Bullet bullet, GameObject other)
        {
            if (!other.TryGetComponent(out TeamComponent team))
            {
                return;
            }

            if (bullet.IsPlayer == team.IsPlayer)
            {
                return;
            }

            if (other.TryGetComponent(out HitPointsComponent hitPoints))
            {
                hitPoints.TakeDamage(bullet.Damage);
            }
        }

        public void OnPause()
        {
            _isPaused = true;
        }

        public void OnResume()
        {
            _isPaused = false;
        }
    }
}