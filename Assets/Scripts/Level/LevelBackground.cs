using System;
using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class LevelBackground : MonoBehaviour, IAwake, IFixedUpdate, IPause, IResume
    {
        private float _startPositionY;

        private float _endPositionY;

        private float _movingSpeedY;

        private float _positionX;

        private float _positionZ;

        private bool _isPaused;

        private Transform _myTransform;

        [SerializeField]
        private Params _params;

        public void AwakeObj()
        {
            _startPositionY = _params.m_startPositionY;
            _endPositionY = _params.m_endPositionY;
            _movingSpeedY = _params.m_movingSpeedY;
            _myTransform = transform;
            var position = _myTransform.position;
            _positionX = position.x;
            _positionZ = position.z;
        }

        public void FixedUpdateObj()
        {
            if (_isPaused)
                return;

            if (_myTransform.position.y <= _endPositionY)
            {
                _myTransform.position = new Vector3(
                    _positionX,
                    _startPositionY,
                    _positionZ
                );
            }

            _myTransform.position -= new Vector3(
                _positionX,
                _movingSpeedY * Time.fixedDeltaTime,
                _positionZ
            );
        }

        public void OnPause()
        {
            _isPaused = true;
        }

        public void OnResume()
        {
            _isPaused = false;
        }

        [Serializable]
        public sealed class Params
        {
            [SerializeField]
            public float m_startPositionY;

            [SerializeField]
            public float m_endPositionY;

            [SerializeField]
            public float m_movingSpeedY;
        }
    }
}