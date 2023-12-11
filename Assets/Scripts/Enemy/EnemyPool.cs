using System.Collections.Generic;
using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class EnemyPool : MonoBehaviour, IAwake, IPause, IResume
    {
        [SerializeField]
        private GameFlowManager _gameFlowManager;

        [Header("Spawn")]
        [SerializeField]
        private EnemyPositions _enemyPositions;

        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private Transform _worldTransform;

        [Header("Pool")]
        [SerializeField]
        private Transform _container;

        [SerializeField]
        private GameObject _prefab;

        private bool _isPaused;

        private readonly Queue<GameObject> _enemyPool = new();
        
        public void AwakeObj()
        {
            for (var i = 0; i < 7; i++)
            {
                var enemy = Instantiate(_prefab, _container);
                _enemyPool.Enqueue(enemy);
            }
        }

        public bool TrySpawnEnemy(out GameObject enemy)
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
            var moveAgent = enemy.GetComponent<EnemyMoveAgent>();
            var attackAgent = enemy.GetComponent<EnemyAttackAgent>();
            moveAgent.SetDestination(attackPosition.position);
            attackAgent.SetTarget(_character);
            _gameFlowManager.AddPausebleObj(moveAgent);
            _gameFlowManager.AddPausebleObj(attackAgent);
            _gameFlowManager.AddFixedUpdatebleObj(moveAgent);
            _gameFlowManager.AddFixedUpdatebleObj(attackAgent);
            return true;
        }

        public void UnspawnEnemy(GameObject enemy)
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