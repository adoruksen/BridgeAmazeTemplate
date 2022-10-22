using System;
using UnityEngine;
using Managers;

namespace Character.StateMachine.Player
{
    [Serializable]
    public class PlayerAmazeState : AmazeState
    {
        private AmazeSystem.AmazeMovement _controller;

        protected override void OnStateEnter(CharacterController controller)
        {
            base.OnStateEnter(controller);
            _controller = controller.Amaze.Movement;
            GameManager.Instance.isAmazeState = true;
        }
        public override void OnStateFixedUpdate(CharacterController controller)
        {
            if (_controller.touchController.Direction == Vector2.up && _controller.collisionSide != "Back")
            {
                _controller.Move(Vector3.forward, Time.fixedDeltaTime);
            }
            else if (_controller.touchController.Direction == Vector2.down && _controller.collisionSide != "Forward")
            {
                _controller.Move(-Vector3.forward, Time.fixedDeltaTime);
            }
            else if (_controller.touchController.Direction == Vector2.right && _controller.collisionSide != "Left")
            {
                _controller.Move(Vector3.right, Time.fixedDeltaTime);
            }
            else if (_controller.touchController.Direction == Vector2.left && _controller.collisionSide != "Right")
            {
                _controller.Move(-Vector3.right, Time.fixedDeltaTime);
            }
        }

        protected override void OnStateExit(CharacterController controller)
        {
            base.OnStateExit(controller);
            GameManager.Instance.isAmazeState = false;
        }
    }
}