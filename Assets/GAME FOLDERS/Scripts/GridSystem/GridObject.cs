using System;
using UnityEngine;

namespace GridSystem
{
    public class GridObject : MonoBehaviour
    {
        public bool IsInGrid { get; protected set; }
        public Action OnGridObjectUsed { get; set; }

        protected virtual void OnDisable()
        {
            RemoveFromGrid();
        }

        public void PlaceOnGrid(Vector3 position)
        {
            transform.position = position;
            transform.rotation = Quaternion.identity;
            IsInGrid = true;
        }

        protected void RemoveFromGrid()
        {
            if (!IsInGrid) return;

            OnGridObjectUsed.Invoke();
            IsInGrid = false;
        }
    }
}