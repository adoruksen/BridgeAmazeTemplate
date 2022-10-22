using System;
using System.Collections.Generic;
using LevelSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public static event Action<LevelController> OnLevelSpawned;

        [ReadOnly] public LevelController Level;

        private void Awake()
        {
            Instance = this;
        }

        [Button]
        public void SpawnLevel(LevelPart[] parts)
        {
            Level = new GameObject("Level").AddComponent<LevelController>();
            var gameAreas = new List<GameAreaManager>();

            var prevArea = parts[0].SetupPart(Level.transform);
            gameAreas.Add(prevArea);
            for (var i = 1; i < parts.Length; i++)
            {
                var area = parts[i].SetupPart(Level.transform, prevArea);
                gameAreas.Add(area);
                prevArea = area;
            }

            Level.GameAreas = gameAreas.ToArray();
            OnLevelSpawned?.Invoke(Level);
        }

        public void DestroyLevel()
        {
            if (Level != null) Destroy(Level.gameObject);
        }
    }
}