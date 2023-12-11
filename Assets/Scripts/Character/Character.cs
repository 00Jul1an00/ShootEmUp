using UnityEngine;

namespace ShootEmUp
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private WeaponComponent _weapon;
        [SerializeField] private MoveComponent _mover;
        [SerializeField] private BulletConfig _bulletConfig;

        public void Move(Vector2 velocity)
        {
            _mover.MoveByRigidbodyVelocity(velocity);
        }

        public void Fire()
        {
            _weapon.Fire(new Args
            {
                isPlayer = true,
                PhysicsLayer = (int)_bulletConfig.PhysicsLayer,
                Color = _bulletConfig.Color,
                Damage = _bulletConfig.Damage,
                Position = _weapon.Position,
                Velocity = _weapon.Rotation * Vector3.up * _bulletConfig.Speed
            });
        }
    }
}