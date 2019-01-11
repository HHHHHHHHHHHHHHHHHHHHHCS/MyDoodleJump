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
    /// 回收的Y
    /// </summary>
    private const float recoveryTileY = 12.5f;

    [field:SerializeField]
    public GameData gameData { get; private set; }

    public static GameData GameData=> Instance.gameData;

    public TileManager tileManager;
    public ItemManager itemManager;

    public ObjectPool<Transform> CoinPool { get; private set; }
    public ObjectPool<Transform> BulletPool { get; private set; }
    public ObjectPool<Transform> EnemyPool { get; private set; }


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

        tileManager = new TileManager();
        itemManager = new ItemManager();


        tileManager.createTileCallback += itemManager.SpawnItem;
    }

    private void Start()
    {
        tileManager.CreateStartTiles();
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

        tileManager.OnUpdate();
        itemManager.OnUpdate();
    }

    /// <summary>
    /// 执行回收
    /// </summary>
    /// <param name="nowY"></param>
    public void DoRecovery(float nowY)
    {
        RecoverY = nowY - recoveryTileY;
        tileManager.RecoveryTile(RecoverY);
        itemManager.RecoveryTile(RecoverY);
    }

    public void PlayerDie()
    {
        GameState = GameState.GameOver;
    }
}