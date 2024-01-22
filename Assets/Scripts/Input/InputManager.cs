using GameFlow;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class InputManager : IUpdate
    {
        public float HorizontalDirection { get; private set; }
        public event System.Action FireRequest;

        public void UpdateObj()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireRequest?.Invoke();
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                HorizontalDirection = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                HorizontalDirection = 1;
            }
            else
            {
                HorizontalDirection = 0;
            }
        }
    }
}