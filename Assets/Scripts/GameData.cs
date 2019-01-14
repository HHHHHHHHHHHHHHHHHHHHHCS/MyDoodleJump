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
/// 物品类型
/// </summary>
public enum ItemType
{
    None = 0,
    Hat,
    Rocket,
}

/// <summary>
/// 金钱的类型,暂时只有硬币
/// </summary>
public enum MoneyType
{
    None=0,
    Coin,
}

public class GameData : ScriptableObject
{
    public const float xMinBorder = -4.5f, xMaxBorder = 4.5f;


    [Space(10), Header("Player")]
    public float playerHorSpeed = 0.1f;
    public float playerFlySpeed = 15f;


    [Space(10), Header("Tile")]
    public float startTilePosY = -4;
    public TileBase tilePrefab;
    public string tileParent = "TileParent";
    public Sprite[] titleSprite;
    public NormalTile normalTile;
    public BrokenTile brokenTile;
    public OnceTile onceTile;
    public SpringTile springTile;
    public MoveHorTile moveHorTile;
    public MoveVerTile moveVerTile;

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


    private float allTileWeight = -1;

    public float AllTileWeight
    {
        get
        {
            if (allTileWeight < 0)
            {
                allTileWeight = normalTile.weight + brokenTile.weight + onceTile.weight
                                + springTile.weight + moveHorTile.weight + moveVerTile.weight;
            }

            return allTileWeight;
        }
    }

    private float allItemWeight = -1;

    public float AllItemWeight
    {
        get
        {
            if (allItemWeight < 0)
            {
                allItemWeight = 1 + hatWeight + rocketWeight;
            }

            return allItemWeight;
        }
    }

    private float allMoneyWeight = -1;

    public float AllMoenyWeight
    {
        get
        {
            if (allMoneyWeight < 0)
            {
                allMoneyWeight = 1 + coinWeight;
            }

            return allMoneyWeight;
        }
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
}