using InteractionSystem;
using System;
using TeamSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using CharacterController = Character.CharacterController;
using DG.Tweening;

namespace StackSystem.FillArea
{
    public class FillObject : MonoBehaviour, IFillObject , IHaveTeam
    {
        private MeshRenderer _renderer;
        [ShowInInspector, ReadOnly] public bool IsInteractable { get; private set; }

        public event Action<Team> OnTeamChanged;
        public Team Team { get; private set; }

        private void Awake()
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
            IsInteractable = true;
        }

        public void AssignTeam(Team team)
        {
            if (team == Team) return;

            Team = team;
            OnTeamChanged?.Invoke(Team);
        }

        public void OnInteractBegin(IInteractor interactor)
        {
            if(!Managers.GameManager.Instance.isAmazeState) return;
            var controller = (AmazeSystem.AmazeController)interactor;
            Interact(controller);
        }

        private void Interact(AmazeSystem.AmazeController controller)
        {
            transform.DOLocalRotate((Vector3.right) * 180, 1).OnUpdate(() =>
            {
                _renderer.material.DOColor(Color.red,1);
            });
            
            SetInteractable(false);
        }

        public void SetInteractable(bool interactable)
        {
            IsInteractable = interactable;
        }
    }
}
