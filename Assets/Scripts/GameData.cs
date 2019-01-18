using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 类型
/// </summary>
public enum ObjectType
{
    Tile = 0,
    Item,
    Coin,
    Enemy,
    Bullet,
}

/// <summary>
/// 游戏状态
/// </summary>
public enum GameState
{
    Ready = 0,
    Pause,
    Running,
    GameOver,
}

/// <summary>
/// [RuntimeInitializeOnLoadMethod()]也能执行Unity Awake等方法
/// 编辑器在点击输入的时候有问题 ,建议用vscode 改 asset
/// 增加难度就变成生成敌人的概率把
/// </summary>
[System.Serializable]
public class GameData : ScriptableObject
{
    public const float xMinBorder = -4.5f, xMaxBorder = 4.5f;
    public const float xSafeMinBorder = -5f, xSafeMaxBorder = 5f;
    public const float hardBase = 1.005f;

    [Space(10),Header("Player")]
    public float playerHorSpeed = 0.1f;
    public float playerFlySpeed = 15f;

    [Space(10), Header("Tile")]
    public float startTilePosY = -4;
    public TileBase tilePrefab;
    public string tileParent = "TileParent";
    public Sprite[] titleSprite;

    public NormalTile normalTile = new NormalTile()
    {
        minHeight = 1,
        maxHeight = 2,
        weight = 80,
    };

    public BrokenTile brokenTile = new BrokenTile()
    {
        minHeight = 1.1f,
        maxHeight = 2,
        weight = 40,
    };

    public OnceTile onceTile = new OnceTile()
    {
        minHeight = 1.2f,
        maxHeight = 2.4f,
        weight = 20,
    };

    public SpringTile springTile = new SpringTile()
    {
        minHeight = 1.3f,
        maxHeight = 2.6f,
        weight = 15
    };

    public MoveHorTile moveHorTile = new MoveHorTile()
    {
        minHeight = 1.4f,
        maxHeight = 2.6f,
        weight = 10,
        distance = 1.6f,
        speed = 1,
    };

    public MoveVerTile moveVerTile = new MoveVerTile()
    {
        minHeight = 1.4f,
        maxHeight = 2.6f,
        weight = 10,
        distance = 2,
        speed = 0.7f,
    };

    [Space(10), Header("Item")]
    public ItemBase itemPrefab;
    public string itemParent = "ItemParent";
    public float hatFlyTime = 1.5f;
    public float rocketFlyTime = 3f;
    public float hatWeight = 0.05f;
    public float rocketWeight = 0.01f;

    [Space(10), Header("Money")]
    public MoneyBase moneyPrefab;
    public string moneyParent = "MoneyParent";
    public int coinValue = 1;
    public float coinWeight = 0.1f;

    [Space(10), Header("Enemy")]
    public EnemyBase enemyPrefab;
    public string enemyParent = "EnemyParent";
    public float enemy1Weight = 0.02f;
    public float enemy2Weight = 0.01f;
    public float enemy3Weight = 0.01f;
    public float enemy2MoveSpeed = 1f;
    public float enemy3MoveSpeed = 0.75f;
    public float enemyMinHeight = 1;
    public float enemyMaxHeight = 3;

    [Space(10), Header("Bullet")]
    public BulletBase bulletPrefab;
    public string bulletParent = "BulletParent";
    public float bulletMoveSpeed = 20f;
    public float bulletNextTime = 0.2f;
    public float bulletDestroyTime = 1f;

    public float AllTileWeight { get; private set; }
    public float AllItemWeight { get; private set; }
    public float AllMoneyWeight { get; private set; }
    public float AllEnemyWeight { get; private set; }

    public float HardCount { get; private set; } = 1;

    public bool IsInit { get; private set; }


    public GameData OnInit()
    {
        if (IsInit)
        {
            return this;
        }

        GeneratorTileWeight();
        GeneratorItemWeight();
        GeneratorMoneyWeight();
        GeneratorEnemyWeight();

        return this;
    }


    public void GeneratorTileWeight()
    {
        brokenTile.weight += normalTile.weight;
        onceTile.weight += brokenTile.weight;
        springTile.weight += onceTile.weight;
        moveHorTile.weight += springTile.weight;
        moveVerTile.weight += moveHorTile.weight;
        AllTileWeight = moveVerTile.weight;
    }

    public void GeneratorItemWeight()
    {
        hatWeight += 1;
        rocketWeight += hatWeight;
        AllItemWeight = rocketWeight;
    }

    public void GeneratorMoneyWeight()
    {
        coinWeight += 1;
        AllMoneyWeight = coinWeight;
    }

    public void GeneratorEnemyWeight()
    {
        AllEnemyWeight = 1 + enemy1Weight + enemy2Weight + enemy3Weight;
    }


    public void AddHard()
    {
        HardCount *= hardBase;
        enemy1Weight *= hardBase;
        enemy2Weight *= hardBase;
        enemy3Weight *= hardBase;
        GeneratorEnemyWeight();
    }


    public GameData Clone()
    {
        var data = MemberwiseClone() as GameData;
        data.OnInit();
        return data;
    }

#if UNITY_EDITOR
    [MenuItem("Data/SaveData")]
    static void CreateExampleAsset()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Game Data"
            , "SaveData", "asset", "Save Game Data");
        var asset = CreateInstance<GameData>();
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.Refresh();
    }
#endif
}

public class Tags
{
    public const string Player = "Player";
    public const string Platform = "Platform";
    public const string Bullet = "Bullet";
}