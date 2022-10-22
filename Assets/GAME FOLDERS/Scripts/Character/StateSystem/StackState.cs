namespace Character.StateMachine
{
    public class StackState : State
    {
        protected override void OnStateEnter(CharacterController controller)
        {
            controller.Movement.UseBounds = true;
            controller.Bumper.IsActive = true;
            controller.Rigidbody.isKinematic = false;
        }

        protected override void OnStateExit(CharacterController controller)
        {
            controller.Movement.UseBounds = false;
            controller.Bumper.IsActive = false;
            controller.Rigidbody.isKinematic = true;
        }
    }
}
