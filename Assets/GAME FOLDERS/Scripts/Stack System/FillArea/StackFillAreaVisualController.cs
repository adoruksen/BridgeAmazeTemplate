using TeamSystem;
using DG.Tweening;
using UnityEngine;

namespace StackSystem.FillArea
{
    public class StackFillAreaVisualController : MonoBehaviour
    {
        private StackFillArea _fillArea;
        [SerializeField] private FillObject[] _fillSlots;
        private int _lastIndex = -1;

        private void Awake()
        {
            _fillArea = GetComponentInParent<StackFillArea>();
        }

        private void OnEnable()
        {
            _fillArea.OnAdded += UpdateGrid;
        }

        private void OnDisable()
        {
            _fillArea.OnAdded -= UpdateGrid;
        }

        public void SetFillObjects(FillObject[] fillSlots)
        {
            _fillSlots = fillSlots;
        }

        public void SetFillObjectsReadyAtStart()
        {
            foreach (var fillObj in _fillSlots)
            {
                fillObj.transform.localScale = Vector3.zero;
            }
        }

        private void UpdateGrid(int index, Team team)
        {
            _fillSlots[index].AssignTeam(team);
            if (_lastIndex >= index) return;
            _fillSlots[index].gameObject.SetActive(true);
            _fillSlots[index].transform.DOScale(Vector3.one, .2f).SetEase(Ease.OutBack, 3f);
            _lastIndex++;
        }
    }
}