using System;
using UnityEngine;

namespace Managers
{
    public abstract class BaseGameManager : MonoBehaviour
    {
        public delegate void GameEvents();

        private const string LevelSaveString = "level";

        public bool ShouldSaveProgress = true;

        public static event GameEvents OnLevelStart;

        public static event GameEvents OnLevelFail;

        public static event GameEvents OnLevelComplete;

        public virtual int GetLevel()
        {
            Debug.LogError("Please override this method first!");
            throw new NotImplementedException();
        }

        public virtual string GetLevelString()
        {
            Debug.LogError("Please override this method first!");
            throw new NotImplementedException();
        }

        public virtual void PreviousLevel()
        {
            Debug.LogError("Please override this method first!");
            throw new NotImplementedException();
        }

        public virtual void RestartLevel()
        {
            Debug.LogError("Please override this method first!");
            throw new NotImplementedException();
        }

        public virtual void SkipLevel()
        {
            Debug.LogError("Please override this method first!");
            throw new NotImplementedException();
        }

        public virtual void JumpToLevel(int targetLevel)
        {
            Debug.LogError("Please override this method first!");
            throw new NotImplementedException();
        }

        public virtual void SaveLevel(int targetLevel)
        {
            if (Application.isEditor && !ShouldSaveProgress)
            {
                Debug.Log("ShouldSaveProgress is false. Saving is disabled.");
                return;
            }

            PlayerPrefs.SetInt("level", targetLevel);
            PlayerPrefs.Save();
        }

        public int GetSavedLevel()
        {
            return PlayerPrefs.GetInt("level");
        }

        protected void LevelStart()
        {
            BaseGameManager.OnLevelStart?.Invoke();
        }

        protected void LevelFail()
        {
            BaseGameManager.OnLevelFail?.Invoke();
        }

        protected void LevelComplete()
        {
            BaseGameManager.OnLevelComplete?.Invoke();
        }
    }
}
