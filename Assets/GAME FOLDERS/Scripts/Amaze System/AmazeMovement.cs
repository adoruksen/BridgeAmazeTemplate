using UnityEngine;

namespace AmazeSystem
{
    public enum Direction { None, Forward, Back, Left, Right }
    public abstract class AmazeMovement : MonoBehaviour
    {
        public Rigidbody Rigidbody { get; private set; }
        public AmazeData Data;
        private Direction direction;
        public TouchController touchController;
        public string collisionSide;

        public float CurrentSpeed { get; protected set; }
        public Vector3 CurrentVelocity { get; protected set; }
        public float CurrentRotation { get; protected set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            touchController = GetComponent<TouchController>();
        }


        public abstract void Move(Vector3 direction, float time);
        public abstract void OnCollision(Collision other);

    }
}

