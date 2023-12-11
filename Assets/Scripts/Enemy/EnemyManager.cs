using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class EnemyManager : MonoBehaviour, IStart
    {
        [SerializeField]
        private EnemyPool _enemyPool;

        [SerializeField]
        private BulletSystem _bulletSystem;

        private readonly HashSet<GameObject> _activeEnemies = new();

        public void StartObj()
        {
            StartCoroutine(SpawnCoroutine());
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
                        var hpComponent = enemy.GetComponent<HitPointsComponent>();
                        var attackAgent = enemy.GetComponent<EnemyAttackAgent>();
                        hpComponent.HpIsEmpty += OnDestroyed;
                        attackAgent.OnFire += OnFire;
                    }
                }
            }
        }

        private void OnDestroyed(GameObject enemy)
        {
            if (_activeEnemies.Remove(enemy))
            {
                enemy.GetComponent<HitPointsComponent>().HpIsEmpty -= OnDestroyed;
                enemy.GetComponent<EnemyAttackAgent>().OnFire -= OnFire;

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