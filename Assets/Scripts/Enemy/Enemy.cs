using UnityEngine;

namespace ShootEmUp
{
    public class Enemy : MonoBehaviour
    {
        [field: SerializeField] public float Countdown { get; private set; }
        [field: SerializeField] public MoveComponent MoveComponent { get; private set; }
        [field: SerializeField] public HitPointsComponent HitPointsComponent { get; private set; }
        [field: SerializeField] public WeaponComponent WeaponComponent { get; private set; }

        public EnemyAttackAgent AttackAgent { get; set; }
    }
}
