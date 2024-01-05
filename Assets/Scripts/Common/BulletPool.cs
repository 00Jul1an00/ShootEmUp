using GameFlow;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletPool
    {
        private readonly Queue<Bullet> _pool = new();
        private readonly HashSet<Bullet> _activeBullets = new();
        
        private readonly Transform _worldTransform;
        private readonly Transform _container;
        private readonly Bullet _prefab;
        private readonly GameFlowManager _gameFlowManager;

        private System.Action<Bullet, Collision2D> _onBulletSpawnAction;

        public BulletPool(int startCount,
            Bullet prefab,
            Transform container,
            Transform worldTransform,
            System.Action<Bullet, Collision2D> onBulletSpawnAction,
            GameFlowManager gameFlowManager)
        {
            _prefab = prefab;
            _container = container;
            _gameFlowManager = gameFlowManager;
            _onBulletSpawnAction = onBulletSpawnAction;
            _worldTransform = worldTransform;

            for (var i = 0; i < startCount; i++)
            {
                var bullet = Object.Instantiate(prefab, container);
                _pool.Enqueue(bullet);
            }
        }

        public Bullet SpawnBullet(Args args)
        {
            if(_pool.TryDequeue(out var bullet))
            {
                bullet.transform.SetParent(_worldTransform);
            }
            else
            {
                bullet = Object.Instantiate(_prefab, _worldTransform);
            }

            _gameFlowManager.AddPausebleObj(bullet);
            bullet.Init(args);

            if (_activeBullets.Add(bullet))
            {
                bullet.OnCollisionEntered += _onBulletSpawnAction;
            }

            return bullet;
        }

        public void DespawnBullet(Bullet bullet)
        {
            if (_activeBullets.Remove(bullet))
            {
                bullet.OnCollisionEntered -= _onBulletSpawnAction;
                bullet.transform.SetParent(_container);
                _pool.Enqueue(bullet);
            }
        }

        public List<Bullet> GetBullets()
        {
            return new(_activeBullets);
        }
    }
}
