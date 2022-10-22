using LevelSystem;
using Managers;
using StackSystem.FillArea;
using TeamSystem;
using AmazeSystem;
using UnityEngine;

namespace Character
{
    public class CharacterAmazeStateTrigger : MonoBehaviour
    {
        private AmazeController _amaze;
        private StackFillArea _fillArea;
        private GameAreaManager _stackArea;

        private void Awake()
        {
            _fillArea = GetComponent<StackFillArea>();
            _stackArea = GetComponentInParent<GameAreaManager>();
        }

        private void OnEnable()
        {
            _fillArea.OnCompleted += UpdateCharacterState;
        }

        private void OnDisable()
        {
            _fillArea.OnCompleted -= UpdateCharacterState;
        }

        public void SetAmaze(AmazeController amaze)
        {
            _amaze = amaze;
        }

        private void UpdateCharacterState(Team team)
        {
            var character = CharacterManager.Instance.GetCharacterByTeam(team);
            character.SetState(character.EnterAmazeState);
            character.Amaze = _amaze;
            _stackArea.OnCharacterExited(character);
        }
    }
}
