using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoSingleton<MainGameManager>
{
    /// <summary>
    /// 初始化对象池的大小
    /// </summary>
    private const int initTileSize = 20;

    /// <summary>
    /// 回收的Y
    /// </summary>
    private const float recoveryTileY = 12.5f;

    /// <summary>
    /// 游戏数据,尽量不要使用
    /// </summary>
    [field: SerializeField]
    private GameData _gameData { get; set; }


    /// <summary>
    /// 玩家当前的金钱
    /// </summary>
    private int money = 0;


    public TileManager TileManager { get; private set; }
    public ItemManager ItemManager { get; private set; }
    public MoneyManager MoneyManager { get; private set; }
    public EnemyManager EnemyManager { get; private set; }
    public BulletManager BulletManager { get; private set; }
    public BackgroundManager BackgroundManager { get; private set; }

    /// <summary>
    /// 游戏数据
    /// </summary>
    public static GameData GameData { get; private set; }

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

    /// <summary>
    /// 玩家的最高分
    /// </summary>
    private float highPlayerY  = 0;

    /// <summary>
    /// 玩家Y的分数的补正
    /// </summary>
    private float playerOffsetY;

    protected override void OnAwake()
    {
        GameData = _gameData.Clone();
        Player = GameObject.Find("Player").GetComponent<Player>();

        TileManager = new TileManager().OnInit();
        ItemManager = new ItemManager().OnInit();
        MoneyManager = new MoneyManager().OnInit();
        EnemyManager = new EnemyManager().OnInit();
        BulletManager = new BulletManager().OnInit();
        BackgroundManager = new BackgroundManager().OnInit();

        TileManager.createTileCallback += ItemManager.SpawnItem;
        TileManager.createTileCallback += MoneyManager.SpawnMoney;
        TileManager.createTileCallback += EnemyManager.SpawnEnemy;
        TileManager.createTileCallback += AddHard;

        TileManager.recoveryCallback += ItemManager.RecoveryBindItem;
        TileManager.recoveryCallback += MoneyManager.RecoveryBindMoney;

        playerOffsetY = -Player.transform.position.y;
    }

    private void Start()
    {
        TileManager.CreateStartTiles();
        Player.Instance.OnInit();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

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

        Player.OnUpdate();
        TileManager.OnUpdate();
        ItemManager.OnUpdate();
        EnemyManager.OnUpdate();
        MoneyManager.OnUpdate();
        BulletManager.OnUpdate();

        UpdateScore();
    }

    private void UpdateScore()
    {
        var nowPosY = Player.transform.position.y + playerOffsetY;
        if (nowPosY > highPlayerY)
        {
            UIManager.Instance.UpdateScore(nowPosY);
        }
    }

    /// <summary>
    /// 执行回收
    /// </summary>
    /// <param name="nowY"></param>
    public void DoRecovery(float nowY)
    {
        RecoverY = nowY - recoveryTileY;
        BackgroundManager.OnUpdate(nowY);
        TileManager.RecoveryTile(RecoverY);
        TileManager.SpawnNeedAddTiles();
    }

    public void PlayerDie()
    {
        GameState = GameState.GameOver;
    }

    public void GetMoney(int val)
    {
        money += val;
    }

    public void AddHard(TileBase tile)
    {
        GameData.AddHard();
    }
}