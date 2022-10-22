using UnityEngine;


namespace AmazeSystem
{
    public class BallMovement : AmazeMovement
    {
        private bool _isMoving;
        public int _coloredWallCounter = 0;
        public int _moveCounter = 0;

        public override void Move(Vector3 direction,float time)
        {
            var ballRay = new Ray(transform.position, direction);
            RaycastHit ballHit;
            Debug.DrawRay(transform.position, direction, Color.black);
            if (Physics.Raycast(ballRay, out ballHit, .6f))
            {
                if (ballHit.collider != null)
                {
                    _moveCounter++;
                    BallSetStop();
                    collisionSide = ReturnDirection(ballHit);
                }
            }
            else
            {
                touchController.BallMove = true;
                transform.Translate(50 * time * direction);
            }
        }

        public override void OnCollision(Collision other)
        {
            if (other.gameObject.CompareTag("floor"))
            {
                _coloredWallCounter++;
            }
        }

        private void BallSetStop()
        {
            touchController.BallMove = false;
            Rigidbody.velocity = Vector3.zero;
            transform.Translate(Vector3.zero);
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }

        private string ReturnDirection(RaycastHit hit)
        {
            Direction hitDirection = Direction.None;

            var myNormal = hit.normal;
            myNormal = hit.transform.TransformDirection(myNormal);

            if (myNormal == hit.transform.forward) { hitDirection = Direction.Forward; }
            if (myNormal == -hit.transform.forward) { hitDirection = Direction.Back; }
            if (myNormal == hit.transform.right) { hitDirection = Direction.Right; }
            if (myNormal == -hit.transform.right) { hitDirection = Direction.Left; }

            return $"{hitDirection}";
        }

        public int ColoredWallCounter
        {
            get => _coloredWallCounter;
            set => _coloredWallCounter = value;
        }

        public int MoveCounter
        {
            get => _moveCounter;
            set => _moveCounter = value;
        }

    }
}

