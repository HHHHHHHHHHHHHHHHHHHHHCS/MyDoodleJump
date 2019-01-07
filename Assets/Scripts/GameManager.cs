using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    /// <summary>
    /// 初始化对象池的大小
    /// </summary>
    private const int initTileSize = 20;
    /// <summary>
    /// 装块回收Y
    /// </summary>
    private const float recoveryTileY = 12.5f;

    public TileBase tilePrefab;
    public Transform tileParent;

    private List<TileBase> showTileList;
    private ObjectPool<TileBase> tilePool;
    private ObjectPool<Transform> coinPool;
    private ObjectPool<Transform> bulletPool;
    private ObjectPool<Transform> enemyPool;
    private ObjectPool<Transform> itemPool;

    /// <summary>
    /// 当前跳板的位置
    /// </summary>
    private float currentTilePosY = GameData.startTilePosY;


    /// <summary>
    /// 游戏状态
    /// </summary>
    public GameState GameState { get; private set; }

    /// <summary>
    /// 跳板最后要回收的位置
    /// </summary>
    public float RecoverY { get; private set; } = float.MinValue;

    /// <summary>
    /// 玩家
    /// </summary>
    public Player Player { get; private set; }

    protected override void OnAwake()
    {
        Player = GameObject.Find("Player").GetComponent<Player>();
        showTileList = new List<TileBase>();
        tilePool = new ObjectPool<TileBase>(tilePrefab, 20);
    }

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            SpawnNewTile();
        }
    }

    private void Update()
    {
        if (GameState == GameState.Ready)
        {
            if (Input.GetMouseButtonUp(0)
                || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.D))
            {
                GameState = GameState.Running;
                Player.Resume();
            }
        }

        if (GameState != GameState.Running)
        {
            return;
        }

        foreach (var tile in showTileList)
        {
            tile.OnUpdate();
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
                pos.y = Random.Range(data.normalTile.minHeight, data.normalTile.maxHeight);
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

        showTileList.Add(temp);
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

    /// <summary>
    /// 判断跳板是否需要回收
    /// </summary>
    public bool NeedRecoveryTile(TileBase tile)
    {
        if (tile.transform.position.y > RecoverY)
        {
            return false;
        }
        return true;
    }


    /// <summary>
    /// 回收跳板用
    /// </summary>
    public void RecoveryTile(float nowY)
    {
        RecoverY = nowY - recoveryTileY;
        int removeIndex = -1;
        int count = showTileList.Count - 1;

        for (int i = 0; i < showTileList.Count; i++)
        {//标记是否可回收
            if (!NeedRecoveryTile(showTileList[i]))
            {
                removeIndex = i - 1;
                break;
            }
            else if (i == count)
            {
                removeIndex = count;
            }
        }

        for (int i = 0; i < removeIndex; i++)
        {//回收
            var tempTile = showTileList[0];
            tempTile.Recovery();
            tilePool.Put(tempTile);
            showTileList.RemoveAt(0);
        }

        for (int i = 0; i < removeIndex; i++)
        {

            //添加新的
            SpawnNewTile();
            //添加道具
            //增加游戏难度
        }
    }

    public void PlayerDie()
    {
        GameState = GameState.GameOver;
    }
}