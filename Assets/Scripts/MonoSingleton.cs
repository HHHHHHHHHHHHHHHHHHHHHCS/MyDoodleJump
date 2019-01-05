using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    protected static string singletonPath;
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (!_instance && !string.IsNullOrEmpty(singletonPath))
            {
                _instance = GameObject.Find(singletonPath).GetComponent<T>();
            }
            return _instance;
        }
        protected set
        {
            _instance = value;
        }
    }

    protected void Awake()
    {
        if (!_instance)
        {
            Instance = this as T;
        }
        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}
