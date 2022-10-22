using System;
using System.Collections.Generic;
using TeamSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using CharacterController = Character.CharacterController;

namespace Managers
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance;

        public static event Action OnCharactersSpawned;

        [SerializeField] private CharacterController _playerCharacter;
        [SerializeField] private float _spacing;

        private List<CharacterController> _characters = new List<CharacterController>();
        [ReadOnly] public CharacterController Player;
        [SerializeField] private int _playerIndex;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            BaseGameManager.OnLevelStart += StartCharacters;
        }

        private void OnDisable()
        {
            BaseGameManager.OnLevelStart -= StartCharacters;
        }

        public void SpawnCharacters(Team[] teams)
        {
            var offset = -((teams.Length - 1f) / 2) * _spacing;
            for (var i = 0; i < teams.Length; i++)
            {
                var position = Vector3.left * (offset + i * _spacing);
                if (i == _playerIndex)
                {
                    SpawnPlayer(teams[i], position);
                }
            }
            OnCharactersSpawned?.Invoke();
        }

        private void SpawnPlayer(Team team, Vector3 position)
        {
            Player = Instantiate(_playerCharacter, position, Quaternion.identity);
            Player.AssignTeam(team);
            _characters.Add(Player);
        }

        private void StartCharacters()
        {
            foreach (var characterController in _characters)
            {
                if (characterController.StackState == null) continue;
                characterController.SetState(characterController.StackState);
            }
        }

        public IReadOnlyList<CharacterController> GetCharacters() => _characters;

        public CharacterController GetCharacterByTeam(Team team)
        {
            for (var i = 0; i < _characters.Count; i++)
            {
                if (_characters[i].Team == team) return _characters[i];
            }

            Debug.Log($"Character With {team.name} Does Not Exist!", this);
            return null;
        }

        public void DestroyCharacter()
        {
            while (_characters.Count > 0)
            {
                var character = _characters[0];
                _characters.RemoveAt(0);
                Destroy(character.gameObject);
            }
        }
    }
}