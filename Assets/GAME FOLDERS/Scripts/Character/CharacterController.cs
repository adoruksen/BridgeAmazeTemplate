using System;
using Character.StateMachine;
using InteractionSystem;
using TeamSystem;
using StackSystem;
using AmazeSystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character
{
    public class CharacterController : MonoBehaviour , IInteractor, IHaveTeam
    {
        [SerializeReference, BoxGroup("Idle", false), HorizontalGroup("Idle/Group")] public State IdleState;
        [SerializeReference, BoxGroup("Stek", false), HorizontalGroup("Stek/Group")] public StackState StackState;
        [SerializeReference, BoxGroup("Fnsh", false), HorizontalGroup("Fnsh/Group")] public FinishState FinishState;
        [SerializeReference, BoxGroup("Winn", false), HorizontalGroup("Winn/Group")] public State WinState;
        [SerializeReference, BoxGroup("Fail", false), HorizontalGroup("Fail/Group")] public State FailState;
        [SerializeReference, BoxGroup("Entr", false), HorizontalGroup("Entr/Group")] public EnterAmazeState EnterAmazeState;
        [SerializeReference, BoxGroup("Amaze", false), HorizontalGroup("Amaze/Group")] public AmazeState AmazeState;
        [SerializeReference, BoxGroup("Exit", false), HorizontalGroup("Exit/Group")] public ExitAmazeState ExitAmazeState;
        [ShowInInspector, ReadOnly, BoxGroup("States", false)] public State CurrentState { get; private set; }

        public Team Team => _team;
        public event Action<Team> OnTeamChanged;
        [SerializeField, DisableInPlayMode] private Team _team;


        public Rigidbody Rigidbody { get; private set; }
        public Interactor Interactor { get; private set; }
        public CharacterMovement Movement { get; private set; }
        public CharacterBumpController Bumper { get; private set; }
        public StackController StackController { get; private set; }
        public CharacterAnimationController Animation { get; private set; }

        public AmazeController Amaze;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Movement = GetComponent<CharacterMovement>();
            Bumper = GetComponent<CharacterBumpController>();
            StackController = GetComponent<StackController>();
            Interactor = GetComponentInChildren<Interactor>();
            Animation = GetComponent<CharacterAnimationController>();
            SetState(IdleState);
        }

        private void Start()
        {
            if (Team != null) OnTeamChanged?.Invoke(Team);
        }

        private void FixedUpdate()
        {
            CurrentState?.OnStateFixedUpdate(this);
        }

        public void AssignTeam(Team team)
        {
            if (team == Team) return;

            _team = team;
            OnTeamChanged?.Invoke(Team);
        }

        public void ExitState()
        {
            CurrentState?.StateExit(this);
        }

        public void SetState(State newState)
        {
            ExitState();
            CurrentState = newState;
            CurrentState.StateEnter(this);
        }

        public Tween ExitAmazeAnimation()
        {
            Amaze = null;
            Rigidbody.position = transform.position;
            var jumpPosition = Movement.Bounds.ClosestPoint(Rigidbody.position);
            return Rigidbody.DOJump(jumpPosition, .25f, 1, .75f);
        }
    }
}

