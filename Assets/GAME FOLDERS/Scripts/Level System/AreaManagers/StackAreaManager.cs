using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GridSystem;
using Managers;
using StackSystem;
using StackSystem.FillArea;
using TeamSystem;
using AmazeSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using CharacterController = Character.CharacterController;
using Random = UnityEngine.Random;

namespace LevelSystem
{
    public class StackAreaManager : GameAreaManager
    {
        [SerializeField] private GridController _grid;
        [SerializeField] private Transform _fillAreaPlacer;

        private List<Team> _teams;
        [SerializeField, BoxGroup("", false)] private StackFillAreaGenerator _fillAreaGenerator;

        private bool _spawnNewObjects;
        [SerializeField] private float _waitDelay;
        private WaitForSeconds _wait;
        private List<StackFillArea> _activeFillAreas;
        private List<FloorRespawner> _floorRespawners;

        private void Awake()
        {
            _wait = new WaitForSeconds(_waitDelay);
            _teams = new List<Team>();
        }

        [Button]
        public void InitializeStackArea(AmazeData[] amazeArray)
        {
            _grid.InitializeGrid();
            _activeFillAreas = _fillAreaGenerator.Generate(_fillAreaPlacer, amazeArray);
            _floorRespawners = new List<FloorRespawner>();
            for (var i = 0; i < _activeFillAreas.Count; i++)
            {
                _floorRespawners.Add(new FloorRespawner(this, _activeFillAreas[i]));
            }
        }

        public override void OnCharacterEntered(CharacterController character)
        {
            AddTeam(character.Team);
            if (!GameManager.Instance.IsPlaying) return;

            character.SetState(character.StackState);
            character.Bumper.IsActive = true;
        }

        public override void OnCharacterExited(CharacterController character)
        {
            RemoveTeam(character.Team);
            CheckFail();
        }

        private void AddTeam(Team team)
        {
            _teams.Add(team);
            var characterCount = CharacterManager.Instance.GetCharacters().Count;
            SpawnTeamObjects(team, (_grid.Count / characterCount));
        }

        private void RemoveTeam(Team team)
        {
            _teams.Remove(team);
            var floor = FloorManager.Instance.GetAvailableObjects(team);
            while (floor.Count > 0)
            {
                FloorManager.Instance.RemoveObject(floor[0]);
            }
        }

        private void CheckFail()
        {
            if (_activeFillAreas.Any(fillArea => !fillArea.Filled)) return;

            foreach (var team in _teams)
            {
                var character = CharacterManager.Instance.GetCharacterByTeam(team);
                if (character != CharacterManager.Instance.Player) continue;

                FinishManager.Instance.FinishLevel();
                return;
            }
        }

        private void SpawnTeamObjects(Team team, int floorAmount)
        {
            var emptySlots = _grid.GetEmptySlotIndices();
            if (emptySlots.Count <= 0) return;

            for (var i = 0; i < floorAmount; i++)
            {
                var index = Random.Range(0, emptySlots.Count);
                var floor = FloorManager.Instance.SpawnObject(team);
                _grid.AddItemToSlot(floor.GetComponent<GridObject>(), emptySlots[index]);
            }
        }

        public void SpawnFloor(Team team)
        {
            var emptySlots = _grid.GetEmptySlotIndices();
            if (emptySlots.Count <= 0 || _teams.Count <= 0) return;

            var slotIndex = Random.Range(0, emptySlots.Count);
            var floor = FloorManager.Instance.SpawnObject(team);

            _grid.AddItemToSlot(floor.GetComponent<GridObject>(), emptySlots[slotIndex]);
        }

        private class FloorRespawner
        {
            private StackAreaManager _manager;
            private StackFillArea _fillArea;

            public FloorRespawner(StackAreaManager manager, StackFillArea fillArea)
            {
                _manager = manager;
                _fillArea = fillArea;
                RegisterFillArea();
            }

            private void RegisterFillArea()
            {
                _fillArea.OnAdded += SpawnFloorOnGrid;
                _fillArea.OnCompleted += UnregisterFillArea;
            }

            private void UnregisterFillArea(Team team)
            {
                _fillArea.OnAdded -= SpawnFloorOnGrid;
                _fillArea.OnCompleted -= UnregisterFillArea;
            }

            private void SpawnFloorOnGrid(int index, Team team)
            {
                _manager.SpawnFloor(team);
            }
        }
    }
}