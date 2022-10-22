using System;
using InteractionSystem;
using Managers;
using TeamSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using CharacterController = Character.CharacterController;
using Random = UnityEngine.Random;

namespace StackSystem
{
    public class Floor : MonoBehaviour, IBeginInteract , IHaveTeam
    {
        public event Action OnCollected;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private float _verticalForce;
        [SerializeField] private float _horizontalForce;
        [ShowInInspector, ReadOnly] public bool IsInteractable { get; private set; }

        public event Action<Team> OnTeamChanged;
        public Team Team => _team;
        [SerializeField, DisableInPlayMode] private Team _team;

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

        public void OnInteractBegin(IInteractor interactor)
        {
            var controller = (CharacterController)interactor;
            if (Team == null)
            {
                Collect(controller);
                AssignTeam(controller.Team);
            }
            else if (controller.Team == Team)
            {
                Collect(controller);
            }
        }

        private void Collect(CharacterController controller)
        {
            OnCollected?.Invoke();
            controller.StackController.AddStack(this);
            SetInteractable(false);
        }

        public void SetLost()
        {
            transform.SetParent(GameManager.Instance.DefaultParent);
            gameObject.SetActive(false);
            AssignTeam(null);
            SetInteractable(true);
            FlingBrick();
        }

        public void SetInteractable(bool interactable)
        {
            _collider.enabled = interactable;
            _rigidbody.isKinematic = !interactable;
            IsInteractable = interactable;
            FloorManager.Instance.SetObjectAvailable(this, interactable);
        }

        private void FlingBrick()
        {
            Vector3 force = Random.insideUnitCircle * _horizontalForce;
            force = new Vector3(force.x, Random.value * _verticalForce, force.y);
            _rigidbody.AddForce(force);
        }

        public void PlaceOnGrid(Vector3 position)
        {
            transform.position = position;
        }
    }
}