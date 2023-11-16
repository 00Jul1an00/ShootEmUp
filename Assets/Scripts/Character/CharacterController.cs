using UnityEngine;

namespace ShootEmUp
{
    public sealed class CharacterController : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private Character _character;

        private void OnEnable()
        {
            _inputManager.FireRequest += OnFireRequest;
        }

        private void OnDisable()
        {
            _inputManager.FireRequest -= OnFireRequest;
        }

        private void FixedUpdate()
        {
            _character.Move(new Vector2(_inputManager.HorizontalDirection, 0) * Time.fixedDeltaTime);
        }

        private void OnFireRequest()
        {
            _character.Fire();
        }
    }
}