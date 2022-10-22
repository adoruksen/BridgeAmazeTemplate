using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if ((Object)_instance == (Object)null)
            {
                _instance = Object.FindObjectOfType<T>();
                if ((Object)_instance == (Object)null)
                {
                    Debug.LogError($"An instance of {typeof(T)} is needed in the scene ");
                }
            }

            return _instance;
        }
    }
}
