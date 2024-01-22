using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class EnemyAttackAgent : IFixedUpdate, IPause, IResume
    {
        public delegate void FireHandler(GameObject enemy, Vector2 position, Vector2 direction);

        public event FireHandler OnFire;

        private readonly GameObject _enemy;
        private readonly WeaponComponent _weaponComponent;
        private readonly EnemyMoveAgent _moveAgent;
        private readonly float _countdown;

        private Character _target;
        private float _currentTime;
        private bool _isPaused;

        public EnemyAttackAgent(GameObject enemy, WeaponComponent weaponComponent, EnemyMoveAgent moveAgent, float countdown)
        {
            _enemy = enemy;
            _weaponComponent = weaponComponent;
            _moveAgent = moveAgent;
            _countdown = countdown;
        }

        public void SetTarget(Character target)
        {
            _target = target;
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
            
            if (!_target.HitPointsComponent.IsHitPointsExists())
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
            OnFire?.Invoke(_enemy, startPosition, direction);
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