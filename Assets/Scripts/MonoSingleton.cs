using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    protected static string singletonPath;
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (!instance && !string.IsNullOrEmpty(singletonPath))
            {
                instance = GameObject.Find(singletonPath).GetComponent<T>();
            }
            return instance;
        }
        protected set
        {
            instance = value;
        }
    }

    protected void Awake()
    {
        if (!instance)
        {
            Instance = this as T;
        }
        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}
