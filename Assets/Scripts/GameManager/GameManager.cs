using UnityEngine;
using GameFlow;
using System.Collections;

namespace ShootEmUp
{
    public sealed class GameManager : IFinishGame, IAwake
    {
        private readonly ICoroutineStarter _coroutiner;
        
        private const float SECONDS_BEFORE_START = 3;

        public GameManager(ICoroutineStarter coroutineStarter)
        {
            _coroutiner = coroutineStarter;
        }

        public void StartGame()
        {
            _coroutiner.CoroutineStarter.StartCoroutine(StartGameCoroutine());
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