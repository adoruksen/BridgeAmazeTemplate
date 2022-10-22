using Managers;
using UnityEngine;
using CharacterController = Character.CharacterController;

namespace CameraSystem
{
    public class CameraConfigSetter : MonoBehaviour
    {
        [SerializeField] private CameraConfig _stackConfig;
        [SerializeField] private CameraConfig _amazeConfig;
        [SerializeField] private CameraConfig _finishConfig;

        private CharacterController _registeredCharacter;

        private void OnEnable()
        {
            GameManager.OnGameInitialized += OnInitialize;
            GameManager.OnGameEnded += OnEnd;
        }

        private void OnDisable()
        {
            GameManager.OnGameInitialized -= OnInitialize;
            GameManager.OnGameEnded -= OnEnd;
        }

        private void OnInitialize()
        {
            RegisterPlayer();
            CameraController.Instance.ResetCamera();
        }

        private void OnEnd()
        {
            UnregisterPlayer();
            SetFinishCamera();
        }

        private void RegisterPlayer()
        {
            _registeredCharacter = CharacterManager.Instance.Player;
            _registeredCharacter.StackState.OnStateEntered += SetStackCamera;
            _registeredCharacter.AmazeState.OnStateEntered += SetDriveCamera;

        }

        private void UnregisterPlayer()
        {
            _registeredCharacter.StackState.OnStateEntered -= SetStackCamera;
            _registeredCharacter.AmazeState.OnStateEntered -= SetDriveCamera;
        }

        private void SetStackCamera(CharacterController obj)
        {
            CameraController.Instance.SetConfig(_stackConfig);
        }

        private void SetDriveCamera(CharacterController obj)
        {
            CameraController.Instance.SetConfig(_amazeConfig);
        }

        private void SetFinishCamera()
        {
            CameraController.Instance.SetConfig(_finishConfig);
        }
    }
}
