using Managers;
using UnityEngine;

namespace CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;
        public CameraFollowTarget FollowTarget;
        public Vector3 Offset;
        public float FollowSpeed;

        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        private Vector3 _defaultPosition;
        private Quaternion _defaultRotation;

        public bool IsActive;
        public bool UseFixedUpdate;

        private void Awake()
        {
            Instance = this;
            _defaultPosition = transform.position;
            _defaultRotation = transform.rotation;
            _targetPosition = _defaultPosition;
            _targetRotation = _defaultRotation;
        }

        private void Update()
        {
            if (UseFixedUpdate) return;

            DoCamera();
        }

        private void FixedUpdate()
        {
            if (!UseFixedUpdate) return;

            DoCamera();
        }

        private void DoCamera()
        {
            if (FollowTarget != null) _targetPosition = FollowTarget.Position + Offset;

            transform.position = Vector3.Lerp(transform.position, _targetPosition, FollowSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, FollowSpeed);
        }

        public void SetTarget(CameraFollowTarget target)
        {
            FollowTarget = target;
        }

        public void SetConfig(CameraConfig config)
        {
            Offset = config.Offset;
            _targetRotation = Quaternion.AngleAxis(config.Rotation, Vector3.right);
        }

        public void ResetCamera()
        {
            transform.position = _defaultPosition;
            transform.rotation = _defaultRotation;
            _targetPosition = _defaultPosition;
            _targetRotation = _defaultRotation;
        }
    }
}

