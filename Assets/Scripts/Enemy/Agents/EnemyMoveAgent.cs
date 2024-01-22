using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class EnemyMoveAgent : IFixedUpdate, IPause, IResume
    {
        public bool IsReached
        {
            get { return _isReached; }
        }

        private readonly MoveComponent _moveComponent;
        private readonly Transform _transform;

        private Vector2 _destination;

        private bool _isReached;
        private bool _isPaused;

        public EnemyMoveAgent(MoveComponent moveComponent, Transform enemyTransform)
        {
            _moveComponent = moveComponent;
            _transform = enemyTransform;
        }

        public void SetDestination(Vector2 endPoint)
        {
            _destination = endPoint;
            _isReached = false;
        }

        public void FixedUpdateObj()
        {
            if (_isReached || _isPaused)
            {
                return;
            }

            var vector = _destination - (Vector2)_transform.position;

            if (vector.magnitude <= 0.25f)
            {
                _isReached = true;
                return;
            }

            var direction = vector.normalized * Time.fixedDeltaTime;
            _moveComponent.MoveByRigidbodyVelocity(direction);
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