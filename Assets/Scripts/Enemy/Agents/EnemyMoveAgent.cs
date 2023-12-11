using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class EnemyMoveAgent : MonoBehaviour, IFixedUpdate, IPause, IResume
    {
        public bool IsReached
        {
            get { return _isReached; }
        }

        [SerializeField] private MoveComponent _moveComponent;

        private Vector2 _destination;

        private bool _isReached;
        private bool _isPaused;

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

            var vector = _destination - (Vector2)transform.position;

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