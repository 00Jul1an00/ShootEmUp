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
                physicsLayer = (int)_bulletConfig.physicsLayer,
                color = _bulletConfig.color,
                damage = _bulletConfig.damage,
                position = _weapon.Position,
                velocity = _weapon.Rotation * Vector3.up * _bulletConfig.speed
            });
        }
    }
}