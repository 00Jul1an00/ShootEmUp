using ShootEmUp;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameFlow
{
    public sealed class GameFlowManager : MonoBehaviour
    {
        [Header("TestHelper")]
        [SerializeField] private HelpersButtons _helpersButtons;
       
        private GameManager _gameManager;
        private OnCharacterKilledObserver _onCharacterKilledObserver;

        private readonly List<IAwake> _awaikebles = new();
        private readonly List<IStart> _startable = new();
        private readonly List<IDisable> _disailables = new();
        private readonly List<IEnable> _enables = new();
        private readonly List<IFixedUpdate> _fixedUpdatbles = new();
        private readonly List<IUpdate> _updatbles = new();
        private readonly List<IPause> _pausebles = new();
        private readonly List<IResume> _resumebles = new();
        private readonly List<IFinishGame> _finishables = new();

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

        [Inject]
        private void Contract(GameManager manager, OnCharacterKilledObserver onCharacterKilledObserver)
        {
            _gameManager = manager;
            _onCharacterKilledObserver = onCharacterKilledObserver;
        }

        [Inject]
        private void InjectFlowInterfaces(List<IAwake> awakebles,
            List<IStart> starts,
            List<IDisable> disables,
            List<IEnable> enables,
            List<IFixedUpdate> fixedUpdates,
            List<IUpdate> updates,
            List<IPause> pauses,
            List<IResume> resumes,
            List<IFinishGame> finishables)
        {
            _awaikebles.AddRange(awakebles);
            _startable.AddRange(starts);
            _disailables.AddRange(disables);
            _enables.AddRange(enables);
            _fixedUpdatbles.AddRange(fixedUpdates);
            _updatbles.AddRange(updates);
            _pausebles.AddRange(pauses);
            _resumebles.AddRange(resumes);
            _finishables.AddRange(finishables);
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