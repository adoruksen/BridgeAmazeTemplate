using Sirenix.OdinInspector;
using UnityEngine;

namespace CameraSystem
{
    public class CameraFollowTarget : MonoBehaviour
    {
        private Transform _transform;
        public Vector3 Position => _transform.position;

        private void Awake()
        {
            _transform = transform;
        }

#if UNITY_EDITOR
        [Button, DisableInEditorMode]
        private void FollowThis()
        {
            CameraController.Instance.SetTarget(this);
        }
#endif
    }
}
