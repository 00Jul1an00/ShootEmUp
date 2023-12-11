using System;
using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour, IPause, IResume
    {
        public event Action<Bullet, Collision2D> OnCollisionEntered;

        public bool IsPlayer { get; private set; }
        public int Damage { get; private set; }

        [SerializeField]
        private Rigidbody2D _rigidbody2D;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private Vector2 _velocity;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEntered?.Invoke(this, collision);
        }

        public void Init(Args args)
        {
            IsPlayer = args.isPlayer;
            Damage = args.Damage;
            SetVelocity(args.Velocity);
            SetPhysicsLayer(args.PhysicsLayer);
            SetPosition(args.Position);
            SetColor(args.Color);
        }

        private void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
            _rigidbody2D.velocity = _velocity;
        }

        private void SetPhysicsLayer(int physicsLayer)
        {
            gameObject.layer = physicsLayer;
        }

        private void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        public void OnResume()
        {
            _rigidbody2D.velocity = _velocity;
        }

        public void OnPause()
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }
}