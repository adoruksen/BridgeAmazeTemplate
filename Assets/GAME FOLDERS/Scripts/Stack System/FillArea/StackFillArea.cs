using System;
using TeamSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StackSystem.FillArea
{
    public class StackFillArea : MonoBehaviour
    {
        public event Action<int, Team> OnAdded;
        public event Action<Team> OnCompleted;

        [ShowInInspector, ReadOnly] private Team[] _grid;
        [ShowInInspector, ReadOnly] private int _size;
        public bool Filled { get; private set; }
        public int Size => _size;

        public void SetSize(int size)
        {
            _size = size;
            _grid = new Team[Size];
        }

        public void AddStack(Team team)
        {
            for (var i = 0; i < Size; i++)
            {
                if (_grid[i] == team) continue;

                SetGridTeam(i, team);
                break;
            }
        }

        public int GetTeamFilledAmount(Team team)
        {
            var count = 0;
            for (var i = 0; i < Size; i++)
            {
                if (_grid[i] == team) count++;
            }
            return count;
        }

        private void SetGridTeam(int i, Team team)
        {
            _grid[i] = team;
            OnAdded?.Invoke(i, team);

            if (i < Size - 1) return;

            Filled = true; // All The grids are filled with same team
            OnCompleted?.Invoke(team);
        }
    }
}
