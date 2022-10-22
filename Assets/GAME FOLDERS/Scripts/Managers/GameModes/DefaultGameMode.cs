using System;
using CameraSystem;
using LevelSystem;
using StackSystem;
using UISystem;
using DG.Tweening;
using UnityEngine;

namespace Managers.GameModes
{
    [CreateAssetMenu(menuName = "Game/GameMode/DefaultGameMode", fileName = "DefaultGameMode", order = -399)]
    public class DefaultGameMode : GameMode
    {
        public LevelConfig[] Levels;

        [SerializeField] private CameraConfig _introConfig;


        [SerializeField] private bool _playCameraAnimation;
        [SerializeField] private float _cameraAnimationSpeed;
        [SerializeField] private AnimationCurve _cameraAnimationCurve;

        public override void InitializeGameMode()
        {
            var config = Levels[GameManager.Instance.GetSavedLevel() % Levels.Length];
            LevelManager.Instance.SpawnLevel(config.Parts);
            CharacterManager.Instance.SpawnCharacters(config.Teams);
            foreach (var character in CharacterManager.Instance.GetCharacters())
            {
                var startArea = LevelManager.Instance.Level.GameAreas[0];
                character.Movement.Bounds = startArea.PlayArea;
                startArea.OnCharacterEntered(character);
                character.SetState(character.IdleState);
            }
            IntroUiController.Instance.ShowInstant();
        }

        public override void StartGameMode(Action levelStart)
        {
            IntroUiController.Instance.Hide();
            if (!_playCameraAnimation)
            {
                StartGame();
                return;
            }
            var sequence = DOTween.Sequence();
            var gameAreas = LevelManager.Instance.Level.GameAreas;
            var target = new GameObject("CameraAnimationTarget").AddComponent<CameraFollowTarget>();
            target.transform.SetParent(GameManager.Instance.DefaultParent);
            CameraController.Instance.SetConfig(_introConfig);
            CameraController.Instance.SetTarget(target);
            var targetPosition = gameAreas[gameAreas.Length - 1].GetNextAreaPosition();
            var duration = targetPosition.z / _cameraAnimationSpeed;
            sequence.Append(target.transform.DOMoveZ(targetPosition.z, duration).SetEase(_cameraAnimationCurve));
            sequence.Append(target.transform.DOMoveZ(-0f, 1f).SetEase(Ease.InOutSine));
            sequence.OnComplete(StartGame);

            void StartGame()
            {
                CameraController.Instance.SetTarget(CharacterManager.Instance.Player.GetComponent<CameraFollowTarget>());
                levelStart.Invoke();
            }
        }

        public override void CompleteGameMode()
        {
            GameManager.Instance.SaveLevel(GameManager.Instance.GetSavedLevel() + 1);
            DOVirtual.DelayedCall(1f, WinUiController.Instance.Show, false);
        }

        public override void FailGameMode()
        {
            DOVirtual.DelayedCall(2f, FailUiController.Instance.Show, false);
        }

        public override void SkipGameMode()
        {
            GameManager.Instance.SaveLevel(GameManager.Instance.GetSavedLevel() + 1);
        }

        public override void DeinitializeGameMode()
        {
            LevelManager.Instance.DestroyLevel();
            CharacterManager.Instance.DestroyCharacter();
            FloorManager.Instance.RemoveAllObjects();
            Destroy(FindObjectOfType<AmazeSystem.BallMovement>().gameObject);
        }
    }
}
