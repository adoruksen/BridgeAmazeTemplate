using System;
using UnityEngine;

namespace LevelSystem
{
    [Serializable]
    public abstract class LevelPart
    {
        public abstract GameAreaManager SetupPart(Transform parent, GameAreaManager previousArea = null);
    }
}
