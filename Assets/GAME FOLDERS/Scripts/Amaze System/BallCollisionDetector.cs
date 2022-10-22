using UnityEngine;

namespace AmazeSystem
{
    public class BallCollisionDetector : MonoBehaviour
    {
        private AmazeMovement _movement;
        private void Awake()
        {
            _movement = GetComponent<AmazeMovement>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            _movement.OnCollision(collision);
        }
    }
}

