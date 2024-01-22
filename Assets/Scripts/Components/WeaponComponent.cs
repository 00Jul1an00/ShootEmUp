using UnityEngine;
using GameFlow;
using Zenject;

namespace ShootEmUp
{
    public sealed class WeaponComponent : MonoBehaviour
    {
        public Vector2 Position
        {
            get { return _firePoint.position; }
        }

        public Quaternion Rotation
        {
            get { return _firePoint.rotation; }
        }

        [SerializeField]
        private Transform _firePoint;

        private BulletSystem _bulletSystem;

        [Inject]
        private void Constract(BulletSystem bulletSystem)
        {
            _bulletSystem = bulletSystem;
        }

        public void Fire(Args bulletArgs)
        {
            _bulletSystem.FlyBulletByArgs(bulletArgs);
        }
    }
}