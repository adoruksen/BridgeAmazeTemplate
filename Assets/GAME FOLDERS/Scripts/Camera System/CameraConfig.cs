using Sirenix.OdinInspector;
using UnityEngine;

namespace CameraSystem
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Game/CameraConfig", order = 0)]
    public class CameraConfig : ScriptableObject
    {
        public Vector3 Offset;
        public float Rotation;

#if UNITY_EDITOR
        [Button, DisableInEditorMode]
        private void ReadFromCamera()
        {
            Offset = CameraController.Instance.FollowTarget.Position - CameraController.Instance.transform.position;
            Rotation = CameraController.Instance.transform.rotation.eulerAngles.x;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
