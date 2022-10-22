using System;
using DG.Tweening;

namespace Character.StateMachine
{
    [Serializable]
    public class ExitAmazeState : State
    {
        private bool _jumpingOut;

        protected override void OnStateEnter(CharacterController controller)
        {
            _jumpingOut = false;
        }

        public override void OnStateFixedUpdate(CharacterController controller)
        {
            if (_jumpingOut) return;
            _jumpingOut = true;
            PlayExitSequence(controller);
        }

        private static void PlayExitSequence(CharacterController controller)
        {
            controller.Amaze.Ball.RemoveCharacter();
            controller.ExitAmazeAnimation().OnComplete(controller.ExitState);
        }
    }
}
