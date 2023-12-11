using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class CharacterController : MonoBehaviour, IEnable, IDisable, IFixedUpdate, IPause, IResume
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private Character _character;

        private bool _isPaused;

        public void Enable()
        {
            _inputManager.FireRequest += OnFireRequest;
        }

        public void Disable()
        {
            _inputManager.FireRequest -= OnFireRequest;
        }

        public void FixedUpdateObj()
        {
            if (_isPaused)
            {
                return;
            }

            _character.Move(new Vector2(_inputManager.HorizontalDirection, 0) * Time.fixedDeltaTime);
        }

        private void OnFireRequest()
        {
            if (_isPaused)
            {
                return;
            }

            _character.Fire();
        }

        public void OnPause()
        {
            _isPaused = true;
        }

        public void OnResume()
        {
            _isPaused = false;
        }
    }
}