using System.Collections.Generic;
using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class BulletSystem : IFixedUpdate, IPause, IResume
    {
        private readonly LevelBounds _levelBounds;
        private readonly List<Bullet> _cache = new();
        private readonly BulletPool _bulletPool;

        private bool _isPaused;

        private const int INITIAL_COUNT = 50;
        
        public BulletSystem(BulletContainer container, 
            Bullet bulletPrefab, 
            Transform worldTransform, 
            LevelBounds levelBounds, 
            GameFlowManager gameFlowManager)
        {
            _levelBounds = levelBounds;
            _bulletPool = new(INITIAL_COUNT, bulletPrefab, container.Container, worldTransform, OnBulletCollision, gameFlowManager);
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