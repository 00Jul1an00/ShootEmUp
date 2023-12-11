using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class EnemyAttackAgent : MonoBehaviour, IFixedUpdate, IPause, IResume
    {
        public delegate void FireHandler(GameObject enemy, Vector2 position, Vector2 direction);

        public event FireHandler OnFire;

        [SerializeField] private WeaponComponent _weaponComponent;
        [SerializeField] private EnemyMoveAgent _moveAgent;
        [SerializeField] private float _countdown;

        private GameObject _target;
        private float _currentTime;
        private bool _isPaused;

        public void SetTarget(GameObject target)
        {
            EnemyAttackAgent enemyAttackAgent = this;
            enemyAttackAgent._target = target;
        }

        public void Reset()
        {
            _currentTime = _countdown;
        }

        public void FixedUpdateObj()
        {
            if(_isPaused)
            {
                return;
            }

            if (!_moveAgent.IsReached)
            {
                return;
            }
            
            if (!_target.GetComponent<HitPointsComponent>().IsHitPointsExists())
            {
                return;
            }

            _currentTime -= Time.fixedDeltaTime;
            if (_currentTime <= 0)
            {
                Fire();
                _currentTime += _countdown;
            }
        }

        private void Fire()
        {
            var startPosition = _weaponComponent.Position;
            var vector = (Vector2)_target.transform.position - startPosition;
            var direction = vector.normalized;
            OnFire?.Invoke(gameObject, startPosition, direction);
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