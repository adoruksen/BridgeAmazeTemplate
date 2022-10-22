using CameraSystem;
using Character;
using Managers;
using Transform = UnityEngine.Transform;

namespace LevelSystem
{
    public class FinishAreaManager : GameAreaManager
    {
        public Transform[] PodiumPlacers;
        public CameraFollowTarget CameraTarget;

        private bool _disabled;

        private void OnEnable()
        {
            GameManager.OnGameEnded += FinishSequence;
        }

        private void OnDisable()
        {
            GameManager.OnGameEnded -= FinishSequence;
        }

        private void FinishSequence()
        {
            CameraController.Instance.FollowTarget = CameraTarget;
            SetStates();
            FillPodium();
        }

        public override void OnCharacterEntered(CharacterController character)
        {
            if (_disabled) return;

            _disabled = true;
            character.Movement.Target = PodiumPlacers[0];
            character.SetState(character.FinishState);
        }

        public void FillPodium()
        {
            var rankOrderedCharacters = FinishManager.Instance.RankedList;

            for (var i = 0; i < PodiumPlacers.Length; i++)
            {
                rankOrderedCharacters[i].transform.position = PodiumPlacers[i].position;
                rankOrderedCharacters[i].transform.rotation = PodiumPlacers[i].rotation;
            }
        }

        private void SetStates()
        {
            var rankOrderedCharacters = FinishManager.Instance.RankedList;
            for (var i = 0; i < rankOrderedCharacters.Count; i++)
            {
                var character = rankOrderedCharacters[i];

                if (character.Amaze != null) character.Amaze.Ball.RemoveCharacter();

                var state = i == 0 ? character.WinState : character.FailState;
                character.SetState(state);
            }
        }
    }
}