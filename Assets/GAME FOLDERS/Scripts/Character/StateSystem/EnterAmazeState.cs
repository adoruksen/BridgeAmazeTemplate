using System;
using DG.Tweening;
using UnityEngine;

namespace Character.StateMachine
{
    [Serializable]
    public class EnterAmazeState : State
    {
        [SerializeField] private float _jumpDistance;

        private bool _sequenceStarted;

        protected override void OnStateEnter(CharacterController controller)
        {
            _sequenceStarted = false;
        }

        public override void OnStateFixedUpdate(CharacterController controller)
        {
            if (_sequenceStarted) return;
            var offset = controller.Amaze.transform.position - controller.Rigidbody.position;
            if (offset.magnitude <= _jumpDistance)
            {
                PlayEnterSequence(controller);
                _sequenceStarted = true;
                return;
            }

            controller.Movement.Move(offset.normalized);
            controller.Movement.Look(offset.normalized);
        }

        private void PlayEnterSequence(CharacterController controller)
        {
            controller.transform.DOJump(controller.Amaze.Ball.Position, .25f, 1, .75f).OnComplete(() =>
            {
                controller.SetState(controller.AmazeState);
                controller.Amaze.Ball.AddCharacter(controller);
            });
        }
    }
}
