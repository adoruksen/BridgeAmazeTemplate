using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace InteractionSystem
{
    public class Interactor : MonoBehaviour
    {
        private IInteractor _controller;
        private readonly List<IStayInteract> _stayInteractables = new List<IStayInteract>(); // StayInteract interface listesi
        public bool CanInteract;

        private void Awake()
        {
            _controller = GetComponentInParent<IInteractor>(); // interactor atilmis objenin parent`in interface IInteractor`u ceker.
        }

        private void FixedUpdate()
        {
            if (!CanInteract || !GameManager.Instance.IsPlaying) return; // Eger ki interact olan bir obje degilse ve IsPlaying degilse (pause ve complete degilse).
            for (var i = 0; i < _stayInteractables.Count; i++)
            {
                if (!_stayInteractables[i].IsInteractable)
                {
                    _stayInteractables.RemoveAt(i);
                    continue;
                }

                _stayInteractables[i].OnInteractStay(_controller);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!CanInteract || !GameManager.Instance.IsPlaying) return;
            if (!other.CompareTag(tag)) return;

            var hasStayInteractable = other.TryGetComponent<IStayInteract>(out var stayInteract);
            if (hasStayInteractable && stayInteract.IsInteractable) _stayInteractables.Add(stayInteract);

            var hasBeginInteractable = other.TryGetComponent<IBeginInteract>(out var interactable);
            if (hasBeginInteractable && interactable.IsInteractable) interactable.OnInteractBegin(_controller);

            var hasFillObjectInteractable = other.TryGetComponent<IFillObject>(out var fillObjectInteract);
            if (hasFillObjectInteractable && fillObjectInteract.IsInteractable) fillObjectInteract.OnInteractBegin(_controller);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!CanInteract || !GameManager.Instance.IsPlaying) return;
            if (!other.CompareTag(tag)) return;

            var hasStayInteractable = other.TryGetComponent<IStayInteract>(out var stayInteract);
            if (hasStayInteractable && _stayInteractables.Contains(stayInteract))
                _stayInteractables.Remove(stayInteract);

            var hasEndInteractable = other.TryGetComponent<IEndInteract>(out var interactable);
            if (hasEndInteractable && interactable.IsInteractable) interactable.OnInteractEnd(_controller);
        }
    }
}