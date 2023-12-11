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

        private readonly Queue<Bullet> _bulletPool = new();
        private readonly HashSet<Bullet> _activeBullets = new();
        private readonly List<Bullet> _cache = new();

        public void AwakeObj()
        {
            for (var i = 0; i < _initialCount; i++)
            {
                var bullet = Instantiate(_prefab, _container);
                _bulletPool.Enqueue(bullet);
            }
        }

        public void FixedUpdateObj()
        {
            if (_isPaused)
                return;

            _cache.Clear();
            _cache.AddRange(_activeBullets);

            for (int i = 0, count = _cache.Count; i < count; i++)
            {
                var bullet = _cache[i];
                if (!_levelBounds.InBounds(bullet.transform.position))
                {
                    RemoveBullet(bullet);
                }
            }
        }

        public void FlyBulletByArgs(Args args)
        {
            if (_isPaused)
            {
                return;
            }

            if (_bulletPool.TryDequeue(out var bullet))
            {
                bullet.transform.SetParent(_worldTransform);
            }
            else
            {
                bullet = Instantiate(_prefab, _worldTransform);
            }

            _gameFlowManager.AddPausebleObj(bullet);
            bullet.Init(args);

            if (_activeBullets.Add(bullet))
            {
                bullet.OnCollisionEntered += OnBulletCollision;
            }
        }

        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            DealDamage(bullet, collision.gameObject);
            RemoveBullet(bullet);
        }

        private void RemoveBullet(Bullet bullet)
        {
            if (_activeBullets.Remove(bullet))
            {
                bullet.OnCollisionEntered -= OnBulletCollision;
                bullet.transform.SetParent(_container);
                _bulletPool.Enqueue(bullet);
            }
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