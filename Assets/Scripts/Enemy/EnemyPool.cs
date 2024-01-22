using System.Collections.Generic;
using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class EnemyPool : IAwake, IPause, IResume
    {
        private readonly GameFlowManager _gameFlowManager;
        private readonly EnemyPositions _enemyPositions;
        private readonly Character _character;
        private readonly Transform _worldTransform;
        private readonly Transform _container;
        private readonly Enemy _prefab;
        private readonly Queue<Enemy> _enemyPool = new();

        private bool _isPaused;
        
        private const int ENEMY_COUNT = 7;

        public EnemyPool(GameFlowManager gameFlowManager,
            EnemyPositions enemyPositions,
            Character character,
            Transform worldTransorm,
            Enemy prefab,
            EnemiesContainer container)
        {
            _gameFlowManager = gameFlowManager;
            _enemyPositions = enemyPositions;
            _character = character;
            _worldTransform = worldTransorm;
            _prefab = prefab;
            _container = container.Container;
        }

        public void AwakeObj()
        {
            for (var i = 0; i < ENEMY_COUNT; i++)
            {
                var enemy = Object.Instantiate(_prefab, _container);
                _enemyPool.Enqueue(enemy);
            }
        }

        public bool TrySpawnEnemy(out Enemy enemy)
        {
            if(_isPaused)
            {
                enemy = null;
                return false;
            }

            if (!_enemyPool.TryDequeue(out enemy))
            {
                return false;
            }

            enemy.transform.SetParent(_worldTransform);

            var spawnPosition = _enemyPositions.RandomSpawnPosition();
            enemy.transform.position = spawnPosition.position;
            
            var attackPosition = _enemyPositions.RandomAttackPosition();
            EnemyMoveAgent moveAgent = new(enemy.MoveComponent, enemy.transform);
            EnemyAttackAgent attackAgent = new(enemy.gameObject, enemy.WeaponComponent, moveAgent, enemy.Countdown);
            moveAgent.SetDestination(attackPosition.position);
            attackAgent.SetTarget(_character);
            enemy.AttackAgent = attackAgent;
            _gameFlowManager.AddPausebleObj(moveAgent);
            _gameFlowManager.AddPausebleObj(attackAgent);
            _gameFlowManager.AddFixedUpdatebleObj(moveAgent);
            _gameFlowManager.AddFixedUpdatebleObj(attackAgent);
            return true;
        }

        public void UnspawnEnemy(Enemy enemy)
        {
            enemy.transform.SetParent(_container);
            _enemyPool.Enqueue(enemy);
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