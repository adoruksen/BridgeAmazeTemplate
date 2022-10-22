namespace Character.StateMachine
{
    public abstract class AmazeState : State
    {
        protected override void OnStateEnter(CharacterController controller)
        {
            controller.Amaze.AssignTeam(controller.Team);
            //controller.Rigidbody.isKinematic = true;
            controller.Rigidbody.detectCollisions = false;
            controller.StackController.LoseAllStack() ;
        }

        protected override void OnStateExit(CharacterController controller)
        {
            //controller.Rigidbody.isKinematic = false;
            controller.Rigidbody.detectCollisions = true;
        }
    }
}
