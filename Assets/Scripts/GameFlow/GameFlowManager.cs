using ShootEmUp;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow
{
    public sealed class GameFlowManager : MonoBehaviour
    {
        [Header("TestHelper")]
        [SerializeField] private HelpersButtons _helpersButtons;
        
        [Space(20)]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private OnCharacterKilledObserver _onCharacterKilledObserver;

        private List<IAwake> _awaikebles = new();
        private List<IStart> _startable = new();
        private List<IDisable> _disailables = new();
        private List<IEnable> _enables = new();
        private List<IFixedUpdate> _fixedUpdatbles = new();
        private List<IUpdate> _updatbles = new();
        private List<IPause> _pausebles = new();
        private List<IResume> _resumebles = new();
        private List<IFinishGame> _finishables = new();

        private void Init()
        {
            _helpersButtons.AddStartButtonListiner(() =>
            {
                _helpersButtons.DeactivateStartButton();
                _gameManager.StartGame();
            });

            _helpersButtons.AddPauseButtonListiner(Pause);
            _helpersButtons.AddResumeButtonListiner(Resume);
            _onCharacterKilledObserver.PlayerDied += GameFinished;
            FillupLists(transform);
        }

        private void FillupLists(Transform transform)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out IAwake awake))
                    _awaikebles.Add(awake);

                if (child.TryGetComponent(out IStart start))
                    _startable.Add(start);

                if (child.TryGetComponent(out IDisable disable))
                    _disailables.Add(disable);

                if (child.TryGetComponent(out IEnable enable))
                    _enables.Add(enable);

                if (child.TryGetComponent(out IFixedUpdate fixedUpdate))
                    _fixedUpdatbles.Add(fixedUpdate);

                if (child.TryGetComponent(out IUpdate update))
                    _updatbles.Add(update);

                if (child.TryGetComponent(out IPause pause))
                    _pausebles.Add(pause);

                if (child.TryGetComponent(out IResume resume))
                    _resumebles.Add(resume);

                if (child.TryGetComponent(out IFinishGame finishGame))
                    _finishables.Add(finishGame);

                if (child.childCount > 0)
                    FillupLists(child);
            }
        }

        private void OnDestroy()
        {
            _onCharacterKilledObserver.PlayerDied -= GameFinished;
        }

        private void Awake()
        {
            Init();

            foreach (var IAwake in _awaikebles)
                IAwake.AwakeObj();
        }

        private void Start()
        {
            foreach (var IStart in _startable)
                IStart.StartObj();
        }

        private void OnEnable()
        {
            foreach (var IEnable in _enables)
                IEnable.Enable();
        }

        private void OnDisable()
        {
            foreach (var IDisable in _disailables)
                IDisable.Disable();
        }

        private void Update()
        {
            for (int i = 0; i < _updatbles.Count; i++)
                _updatbles[i].UpdateObj();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdatbles.Count; i++)
                _fixedUpdatbles[i].FixedUpdateObj();
        }

        private void Pause()
        {
            foreach (var IPause in _pausebles)
                IPause.OnPause();
        }

        private void Resume()
        {
            foreach (var IResume in _resumebles)
                IResume.OnResume();
        }

        private void GameFinished()
        {
            foreach (var IFinishGame in _finishables)
                IFinishGame.OnFinishGame();
        }

        public void AddPausebleObj(IPause obj)
        {
            _pausebles.Add(obj);
            _resumebles.Add(obj as IResume);
        }

        public void AddFixedUpdatebleObj(IFixedUpdate obj)
        {
            _fixedUpdatbles.Add(obj);
        }
    }
}