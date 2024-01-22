using GameFlow;
using System;
using UnityEngine;
using Zenject;

namespace ShootEmUp
{
    public class OnCharacterKilledObserver : IEnable, IDisable
    {
        private HitPointsComponent _characterHitPoints;

        public event Action PlayerDied;

        public OnCharacterKilledObserver(HitPointsComponent characterHitPoints)
        {
            _characterHitPoints = characterHitPoints;
        }

        public void Enable()
        {
            _characterHitPoints.HpIsEmpty += OnHpIsEmpty;
        }

        public void Disable()
        {
            _characterHitPoints.HpIsEmpty -= OnHpIsEmpty;
        }

        private void OnHpIsEmpty(GameObject character)
        {
            PlayerDied?.Invoke();
        }
    }
}