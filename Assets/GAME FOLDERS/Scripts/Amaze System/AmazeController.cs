using InteractionSystem;
using TeamSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace AmazeSystem
{
    public class AmazeController : MonoBehaviour, IInteractor , IHaveTeam
    {
        public event Action<Team> OnTeamChanged;
        public Team Team => _team;
        [SerializeField, DisableInPlayMode] private Team _team;

        //public AnimatorOverrideController CharacterAnimator;
        public AmazeMovement Movement { get; private set; }
        public AmazeBall Ball { get; private set; }


        private void Awake()
        {
            Movement = GetComponent<AmazeMovement>();
            Ball = GetComponentInChildren<AmazeBall>();
        }

        private void Start()
        {
            if (Team != null) OnTeamChanged?.Invoke(Team);
        }

        public void AssignTeam(Team team)
        {
            if (team == Team) return;

            _team = team;
            OnTeamChanged?.Invoke(Team);
        }
    }
}
