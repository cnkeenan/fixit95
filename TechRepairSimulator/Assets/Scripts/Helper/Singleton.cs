using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ported over from https://wiki.unity3d.com/index.php/Singleton
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shuttingDown = false;
    private static object __lock = new object();
    private static T instance;

    public static T Instance
    {
        get
        {
            if (shuttingDown)
            {
                return null;
            }

            lock (__lock)
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return instance;
            }

        }
    }
    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }
 
 
    private void OnDestroy()
    {
        shuttingDown = true;
    }
}
