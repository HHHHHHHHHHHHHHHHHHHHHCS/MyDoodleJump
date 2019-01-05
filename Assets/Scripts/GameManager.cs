using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private const int initTileSize = 20;


    public TileBase tilePrefab;
    public Transform tileParent;

    private ObjectPool<TileBase> tilePool;
    private ObjectPool<Transform> coinPool;
    private ObjectPool<Transform> bulletPool;
    private ObjectPool<Transform> enemyPool;
    private ObjectPool<Transform> itemPool;

    private float currentTilePosY = GameData.startTilePosY;

    protected override void OnAwake()
    {
        tilePool = new ObjectPool<TileBase>(tilePrefab, 20);
    }

    private void Start()
    {
        for (int i= 0;i < 60;i++)
        {
            SpawnNewTile();
        }
    }

    public TileBase SpawnNewTile()
    {
        var temp = tilePool.Get();
        var tileType = (TileType)GetTileType();
        Vector2 pos = new Vector2(Random.Range(GameData.xMinBorder, GameData.xMaxBorder), currentTilePosY);

        var data = GameData.Instance;

        switch (tileType)
        {
            case TileType.NormalTile:
                pos.y = Random.Range(data.normalTile.minHeight,data.normalTile.maxHeight);
                break;
            case TileType.BrokenTile:
                pos.y = Random.Range(data.brokenTile.minHeight, data.brokenTile.maxHeight);

                break;
            case TileType.OnceTile:
                pos.y = Random.Range(data.onceTile.minHeight, data.onceTile.maxHeight);

                break;
            case TileType.SpringTile:
                pos.y = Random.Range(data.springTile.minHeight, data.springTile.maxHeight);

                break;
            case TileType.MoveHorTile:
                pos.y = Random.Range(data.moveHorTile.minHeight, data.moveHorTile.maxHeight);

                break;
            case TileType.MoveVerTile:
                pos.y = Random.Range(data.moveVerTile.minHeight, data.moveVerTile.maxHeight);

                break;
            default:
                break;
        }

        pos.y += currentTilePosY;

        temp.Init(tileType, pos);

        currentTilePosY = pos.y;
        return temp;
    }

    /// <summary>
    /// 得到跳板的种类
    /// </summary>
    private int GetTileType()
    {
        var data = GameData.Instance;

        float rand = Random.Range(0, data.SumAllWeight);
        if (rand < data.normalTile.weight)
        {
            return 0;
        }

        rand -= data.normalTile.weight;
        if (rand < data.brokenTile.weight)
        {
            return 1;
        }

        rand -= data.brokenTile.weight;
        if (rand < data.onceTile.weight)
        {
            return 2;
        }

        rand -= data.onceTile.weight;
        if (rand < data.springTile.weight)
        {
            return 3;
        }

        rand -= data.springTile.weight;
        if (rand < data.moveHorTile.weight)
        {
            return 4;
        }

        rand -= data.moveHorTile.weight;
        if (rand <= data.moveVerTile.weight)
        {
            return 5;
        }

        return 0;
    }
}