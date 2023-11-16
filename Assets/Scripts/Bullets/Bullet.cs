using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet, Collision2D> OnCollisionEntered;

        public bool IsPlayer { get; private set; }
        public int Damage { get; private set; }

        [SerializeField]
        private Rigidbody2D _rigidbody2D;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            this.OnCollisionEntered?.Invoke(this, collision);
        }

        public void Init(Args args)
        {
            IsPlayer = args.isPlayer;
            Damage = args.damage;
            SetVelocity(args.velocity);
            SetPhysicsLayer(args.physicsLayer);
            SetPosition(args.position);
            SetColor(args.color);
        }

        private void SetVelocity(Vector2 velocity)
        {
            this._rigidbody2D.velocity = velocity;
        }

        private void SetPhysicsLayer(int physicsLayer)
        {
            this.gameObject.layer = physicsLayer;
        }

        private void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }

        private void SetColor(Color color)
        {
            this._spriteRenderer.color = color;
        }
    }
}