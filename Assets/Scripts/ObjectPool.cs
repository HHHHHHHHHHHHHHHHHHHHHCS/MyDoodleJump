using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private T prefab;
    private Transform parent;
    private Queue<T> objPool;

    public ObjectPool()
    {

    }

    public ObjectPool(T itemPrefab, int initSize, string prefabParent = null)
    {
        Transform ts = null;
        if (!string.IsNullOrEmpty(prefabParent))
        {
            ts = new GameObject(prefabParent).transform;
        }

        Init(itemPrefab, initSize, ts);
    }

    public ObjectPool(T itemPrefab, int initSize, Transform prefabParent = null)
    {
        Init(itemPrefab, initSize, prefabParent);
    }

    public void Init(T itemPrefab, int initSize, Transform prefabParent = null)
    {
        objPool = new Queue<T>();
        prefab = itemPrefab;
        parent = prefabParent;
        for (int i = 0; i < initSize; i++)
        {
            var item = Instantiate();
            Put(item);
        }
    }


    private T Instantiate()
    {
        if (prefab)
        {
            if (parent)
            {
                return Object.Instantiate(prefab, parent);
            }
            else
            {
                return Object.Instantiate(prefab);
            }
        }

        Debug.Log("Prefab is null can't Instantiate");
        return null;
    }

    public T Get()
    {
        if (objPool.Count > 0)
        {
            return objPool.Dequeue();
        }
        else
        {
            return Instantiate();
        }
    }

    public void Put(T item)
    {
        item.gameObject.SetActive(false);
        objPool.Enqueue(item);
    }
}
