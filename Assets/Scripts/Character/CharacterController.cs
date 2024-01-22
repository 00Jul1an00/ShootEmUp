using UnityEngine;
using GameFlow;

namespace ShootEmUp
{
    public sealed class CharacterController : IEnable, IDisable, IFixedUpdate, IPause, IResume
    {
        private readonly InputManager _inputManager;
        private readonly Character _character;

        public CharacterController(InputManager inputManager, Character character)
        {
            _inputManager = inputManager;
            _character = character;
        }

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