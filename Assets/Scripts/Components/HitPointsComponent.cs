using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class HitPointsComponent : MonoBehaviour
    {
        [SerializeField] private int _hitPoints;

        public event Action<GameObject> HpIsEmpty;        
        
        public bool IsHitPointsExists() 
        {
            return _hitPoints > 0;
        }

        public void TakeDamage(int damage)
        {
            _hitPoints -= damage;
            if (_hitPoints <= 0)
            {
                HpIsEmpty?.Invoke(gameObject);
            }
        }
    }
}