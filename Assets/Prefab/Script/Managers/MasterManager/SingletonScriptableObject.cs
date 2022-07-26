using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();

                if (results.Length == 0)
                {
                    Debug.LogError($"SingletonScriptableObject -> Instance -> result length is 0 for type {typeof(T)}.");
                    return null;
                }
                if (results.Length > 1)
                {
                    Debug.LogError($"SingletonScriptableObject -> Instance -> result length is greather than 1 for type {typeof(T)}.");
                    return null;
                }

                instance = results[0];
            }
            return instance;
        }
    }
}
