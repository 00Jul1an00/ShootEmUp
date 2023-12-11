using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class WeaponComponent : MonoBehaviour, IAwake
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

        public void Fire(Args bulletArgs)
        {
            _bulletSystem.FlyBulletByArgs(bulletArgs);
        }

        public void AwakeObj()
        {
            _bulletSystem = FindObjectOfType<BulletSystem>();
        }
    }
}