using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Managers;
using UnityEngine;

namespace StackSystem
{
    public class StackVisualController : MonoBehaviour
    {
        private StackController _stackController;

        [ShowInInspector] private readonly Stack<Floor> _stackedObjects = new Stack<Floor>();
        [SerializeField] private Transform _stackParent;
        [SerializeField] private float _distance;

        private void Awake()
        {
            _stackController = GetComponent<StackController>();
        }

        private void OnEnable()
        {
            _stackController.OnStackAdded += UpdateVisualAdded;
            _stackController.OnStackUsed += UpdateVisualUsed;
            _stackController.OnStackLost += UpdateVisualLostAll;
        }

        private void OnDisable()
        {
            _stackController.OnStackAdded -= UpdateVisualAdded;
            _stackController.OnStackUsed -= UpdateVisualUsed;
            _stackController.OnStackLost -= UpdateVisualLostAll;
        }

        private void UpdateVisualAdded(Floor obj)
        {
            _stackedObjects.Push(obj);
            var objTransform = obj.transform;
            objTransform.SetParent(_stackParent);
            objTransform.localPosition = Vector3.up * (_stackController.Stack * _distance);
            objTransform.localRotation = Quaternion.identity;
        }

        private void UpdateVisualUsed()
        {
            var obj = _stackedObjects.Pop();
            obj.transform.DOScale(Vector3.zero, .2f).OnComplete(() =>
            {
                FloorManager.Instance.RemoveObject(obj);
                obj.transform.localScale = Vector3.one;
            });
        }

        private void UpdateVisualLostAll()
        {
            while (_stackedObjects.Count > 0)
            {
                var obj = _stackedObjects.Pop();
                obj.SetLost();
            }
        }
    }
}