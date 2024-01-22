using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFlow;
using Zenject;

namespace ShootEmUp
{
    public sealed class EnemyManager : IStart
    {
        private readonly EnemyPool _enemyPool;
        private readonly BulletSystem _bulletSystem;
        private readonly ICoroutineStarter _coroutiner;

        private readonly HashSet<Enemy> _activeEnemies = new();

        public EnemyManager(BulletSystem bulletSystem, EnemyPool pool, ICoroutineStarter coroutineStarter)
        {
            _bulletSystem = bulletSystem;
            _enemyPool = pool;
            _coroutiner = coroutineStarter;
        }

        public void StartObj()
        {
            _coroutiner.CoroutineStarter.StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                if (_enemyPool.TrySpawnEnemy(out var enemy))
                {
                    if (_activeEnemies.Add(enemy))
                    {
                        var hpComponent = enemy.HitPointsComponent;
                        var attackAgent = enemy.AttackAgent;
                        hpComponent.HpIsEmpty += OnDestroyed;
                        attackAgent.OnFire += OnFire;
                    }
                }
            }
        }

        private void OnDestroyed(GameObject enemyGO)
        {
            var enemy = enemyGO.GetComponent<Enemy>();

            if (_activeEnemies.Remove(enemy))
            {
                enemy.HitPointsComponent.HpIsEmpty -= OnDestroyed;
                enemy.AttackAgent.OnFire -= OnFire;

                _enemyPool.UnspawnEnemy(enemy);
            }
        }

        private void OnFire(GameObject enemy, Vector2 position, Vector2 direction)
        {
            _bulletSystem.FlyBulletByArgs(new Args
            {
                isPlayer = false,
                PhysicsLayer = (int)PhysicsLayer.ENEMY_BULLET,
                Color = Color.red,
                Damage = 1,
                Position = position,
                Velocity = direction * 2.0f
            });
        }
    }
}