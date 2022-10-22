using System;
using Managers.GameModes;
using UnityEngine;

namespace Managers
{
    public class GameManager : BaseGameManager
    {
        public static event GameEvents OnGameInitialized;
        public static event GameEvents OnGameEnded;

        public static GameManager Instance;

        [SerializeField] private GameMode _defaultGameMode;

        private GameMode _currentGameMode;
        public Transform DefaultParent;

        public bool IsPlaying { get; private set; }
        public bool isAmazeState;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializeGameMode(_defaultGameMode);
        }

        public void InitializeGameMode(GameMode gameMode)
        {
            if (_currentGameMode != null) _currentGameMode.DeinitializeGameMode();
            _currentGameMode = gameMode;
            _currentGameMode.InitializeGameMode();
            LevelInitialize();
        }

        public void StartGameMode()
        {
            _currentGameMode.StartGameMode(LevelStart);
            IsPlaying = true;
        }

        public void CompleteGameMode()
        {
            LevelEnd();
            LevelComplete();
            IsPlaying = false;
            _currentGameMode.CompleteGameMode();
        }

        public override void RestartLevel()
        {
            JumpToLevel(GetSavedLevel());
        }

        public override void SkipLevel()
        {
            JumpToLevel(GetSavedLevel() + 1);
        }

        public override void JumpToLevel(int targetLevel)
        {
            SaveLevel(targetLevel);
            InitializeGameMode(_currentGameMode);
        }

        public override void PreviousLevel()
        {
            JumpToLevel(GetSavedLevel() - 1);
        }

        public void FailGameMode()
        {
            IsPlaying = false;
            _currentGameMode.FailGameMode();
            LevelEnd();
            LevelFail();
        }

        public override int GetLevel()
        {
            return GetSavedLevel();
        }

        public override string GetLevelString()
        {
            return GetLevel().ToString();
        }

        protected void LevelInitialize()
        {
            OnGameInitialized?.Invoke();
        }

        protected void LevelEnd()
        {
            OnGameEnded?.Invoke();
        }
    }
}
