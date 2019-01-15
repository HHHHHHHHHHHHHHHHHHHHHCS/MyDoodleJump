using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TileManager
{
    public List<TileBase> ShowTileList { get; private set; }
    public ObjectPool<TileBase> TilePool { get; private set; }

    public event Action<TileBase> createTileCallback;
    public event Action<TileBase> recoveryCallback;

    private GameData gameData;

    /// <summary>
    /// 当前跳板的位置
    /// </summary>
    private float currentTilePosY;

    /// <summary>
    /// 需要添加的tiles
    /// </summary>
    private int needAddTiles;

    private Transform platform;


    public TileManager OnInit()
    {
        gameData = MainGameManager.GameData;
        currentTilePosY = gameData.startTilePosY;

        ShowTileList = new List<TileBase>();
        TilePool = new ObjectPool<TileBase>(gameData.tilePrefab, 20, gameData.tileParent);

        platform = GameObject.Find("Platform").transform;
        return this;
    }

    public void CreateStartTiles()
    {
        for (int i = 0; i < 20; i++)
        {
            SpawnNewTile();
        }
    }

    public void OnUpdate()
    {
        foreach (var tile in ShowTileList)
        {
            tile.OnUpdate();
        }
    }

    /// <summary>
    /// 生成跳板
    /// </summary>
    public TileBase SpawnNewTile()
    {
        var temp = TilePool.Get();
        var tileType = (TileType) GetTileType();
        Vector2 pos = new Vector2(Random.Range(GameData.xMinBorder, GameData.xMaxBorder), currentTilePosY);

        switch (tileType)
        {
            case TileType.NormalTile:
                pos.y = Random.Range(gameData.normalTile.minHeight, gameData.normalTile.maxHeight);
                break;
            case TileType.BrokenTile:
                pos.y = Random.Range(gameData.brokenTile.minHeight, gameData.brokenTile.maxHeight);

                break;
            case TileType.OnceTile:
                pos.y = Random.Range(gameData.onceTile.minHeight, gameData.onceTile.maxHeight);

                break;
            case TileType.SpringTile:
                pos.y = Random.Range(gameData.springTile.minHeight, gameData.springTile.maxHeight);

                break;
            case TileType.MoveHorTile:
                pos.y = Random.Range(gameData.moveHorTile.minHeight, gameData.moveHorTile.maxHeight);

                break;
            case TileType.MoveVerTile:
                pos.y = Random.Range(gameData.moveVerTile.minHeight, gameData.moveVerTile.maxHeight);

                break;
            default:
                break;
        }

        pos.y += currentTilePosY;

        temp.Init(tileType, pos);

        currentTilePosY = pos.y;

        ShowTileList.Add(temp);

        createTileCallback?.Invoke(temp);

        return temp;
    }

    /// <summary>
    /// 得到跳板的种类
    /// 其实也可以刚开始直接把权重加起来,进行<=比较
    /// </summary>
    private int GetTileType()
    {
        float rand = Random.Range(0, gameData.AllTileWeight);
        if (rand <= gameData.normalTile.weight)
        {
            return 0;
        }

        if (rand <= gameData.brokenTile.weight)
        {
            return 1;
        }

        if (rand <= gameData.onceTile.weight)
        {
            return 2;
        }

        if (rand <= gameData.springTile.weight)
        {
            return 3;
        }

        if (rand <= gameData.moveHorTile.weight)
        {
            return 4;
        }

        if (rand <= gameData.moveVerTile.weight)
        {
            return 5;
        }

        return 0;
    }

    /// <summary>
    /// 判断跳板是否需要回收
    /// </summary>
    public bool NeedRecoveryTile(TileBase tile)
    {
        if (tile.transform.position.y > MainGameManager.Instance.RecoverY)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 判断跳板是否需要回收
    /// </summary>
    public bool NeedRecoveryTile(TileBase tile, float checkPosY)
    {
        if (tile.transform.position.y > checkPosY)
        {
            return false;
        }

        return true;
    }

    public void SpawnNeedAddTiles()
    {
        for (int i = 1; i <= needAddTiles; i++)
        {
            //增加游戏难度
            //添加新的
            SpawnNewTile();
            //添加道具
        }

        needAddTiles = 0;
    }


    /// <summary>
    /// 回收跳板用
    /// </summary>
    public void RecoveryTile(float checkPosY)
    {
        if (platform && platform.position.y < checkPosY)
        {
            Object.Destroy(platform.gameObject);
        }

        int removeIndex = -1;
        int count = ShowTileList.Count - 1;

        for (int i = 0; i < ShowTileList.Count; i++)
        {
            //标记是否可回收
            if (!NeedRecoveryTile(ShowTileList[i], checkPosY))
            {
                removeIndex = i - 1;
                break;
            }
            else if (i == count)
            {
                removeIndex = count;
            }
        }

        for (int i = 0; i <= removeIndex; i++)
        {
            //回收
            var tempTile = ShowTileList[0];
            tempTile.Recovery();
            recoveryCallback?.Invoke(tempTile);
            TilePool.Put(tempTile);
            ShowTileList.RemoveAt(0);
        }

        needAddTiles += removeIndex + 1;
    }
}