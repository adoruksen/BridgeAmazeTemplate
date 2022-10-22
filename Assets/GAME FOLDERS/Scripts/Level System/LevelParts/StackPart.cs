using System;
using AmazeSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LevelSystem
{
    [Serializable]
    public class StackPart : LevelPart
    {
        [SerializeField] private StackAreaManager _manager;
        public AmazeData[] AmazeArray;

        public override GameAreaManager SetupPart(Transform parent, GameAreaManager previousArea = null)
        {
            var manager = Object.Instantiate(_manager, parent);
            if (previousArea != null)
            {
                var position = previousArea.GetNextAreaPosition();
                manager.MoveArea(position);
            }

            manager.InitializeStackArea(AmazeArray);
            return manager;
        }
    }
}