using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private const int initTileSize = 20;

    public TileBase tilePrefab;
    public Transform tileParent;

    private ObjectPool<TileBase> tilePool;
    private ObjectPool<GameObject> coinPool;
    private ObjectPool<GameObject> bulletPool;
    private ObjectPool<GameObject> enemyPool;
    private ObjectPool<GameObject> itemPool;

    protected override void OnAwake()
    {
        tilePool = new ObjectPool<TileBase>(tilePrefab, 20);
    }


    public TileBase GetTile()
    {
        return tilePool.Get();
    }
}