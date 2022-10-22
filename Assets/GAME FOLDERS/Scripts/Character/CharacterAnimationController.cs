using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Character
{
    public class CharacterAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rigidbody;

        [SerializeField] private GameObject[] _model;
        private RigBuilder _rigBuilder;

        private bool _ikActive;
        [SerializeField] private Transform[] _ikNode;
        private readonly Transform[] _ikTarget = new Transform[2];

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Win = Animator.StringToHash("Win");
        private static readonly int Fail = Animator.StringToHash("Fail");
        private static readonly int AmazeEnter = Animator.StringToHash("AmazeEnter");
        private static readonly int AmazeExit = Animator.StringToHash("AmazeExit");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigBuilder = GetComponentInChildren<RigBuilder>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            _animator.SetFloat(Speed, _rigidbody.velocity.magnitude);

            if (!_ikActive) return;
            _ikNode[0].position = _ikTarget[0].position;
            _ikNode[1].position = _ikTarget[1].position;
        }

        public void SetModelActive(bool isActive)
        {
            foreach (var model in _model)
            {
                model.SetActive(isActive);
            }
        }


        public void TriggerFall() => _animator.SetTrigger(Fall);

        public void SetAnimator(AnimatorOverrideController animator) => _animator.runtimeAnimatorController = animator;

        public void EnterAmaze() => _animator.SetTrigger(AmazeEnter);

        public void ExitAmaze() => _animator.SetTrigger(AmazeExit);

        public void SetWin() => _animator.SetTrigger(Win);

        public void SetFail() => _animator.SetTrigger(Fail);

        public void SetFootIkActive(bool isActive)
        {
            _ikActive = isActive;
            _rigBuilder.layers[0].active = isActive;
        }
        public void SetFootIkTarget(Transform leftPedal, Transform rightPedal)
        {
            _ikTarget[0] = leftPedal;
            _ikTarget[1] = rightPedal;
        }
    }
}
