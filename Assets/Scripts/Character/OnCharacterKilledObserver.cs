using System;
using UnityEngine;

namespace ShootEmUp
{
    public class OnCharacterKilledObserver : MonoBehaviour
    {
        [SerializeField] private HitPointsComponent _characterHitPoints;
        [SerializeField] private GameManager _gameManager;

        public event Action PlayerDied;

        private void OnEnable()
        {
            _characterHitPoints.HpIsEmpty += OnHpIsEmpty;
        }

        private void OnDisable()
        {
            _characterHitPoints.HpIsEmpty -= OnHpIsEmpty;
        }

        private void OnHpIsEmpty(GameObject character)
        {
            PlayerDied?.Invoke();
        }
    }
}