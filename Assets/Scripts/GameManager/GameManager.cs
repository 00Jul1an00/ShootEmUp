using UnityEngine;
using GameFlow;
using System.Collections;

namespace ShootEmUp
{
    public sealed class GameManager : MonoBehaviour, IFinishGame, IAwake
    {
        private const float SECONDS_BEFORE_START = 3;

        public void StartGame()
        {
            StartCoroutine(StartGameCoroutine());
        }

        public void OnFinishGame()
        {
            Debug.Log("Game over!");
            Time.timeScale = 0;
        }

        public void AwakeObj()
        {
            Time.timeScale = 0;
        }

        private IEnumerator StartGameCoroutine()
        {
            float timer = SECONDS_BEFORE_START;

            while (timer > 0)
            {
                yield return new WaitForSecondsRealtime(1);
                Debug.Log(timer--);
            }

            Time.timeScale = 1;
        }
    }
}