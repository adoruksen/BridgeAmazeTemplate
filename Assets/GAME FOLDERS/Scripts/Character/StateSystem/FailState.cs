using DG.Tweening;

namespace Character.StateMachine
{
    public class FailState : State
    {
        protected override void OnStateEnter(CharacterController controller)
        {
            controller.DOKill();
            controller.Animation.SetFail();
            controller.Rigidbody.isKinematic = true;
        }
    }
}
