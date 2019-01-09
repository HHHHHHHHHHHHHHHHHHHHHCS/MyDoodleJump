using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

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

public class GameData : ScriptableObject
{
    public const float xMinBorder = -4.5f, xMaxBorder = 4.5f;
    

    [Space(10), Header("Player")]

    public float playerHorSpeed = 0.1f;
    public float playerFlySpeed = 5f;


    [Space(10), Header("Tile")]

    public float startTilePosY = -4;
    public TileBase tilePrefab;
    public string tileParent;
    public Sprite[] titleSprite;
    public NormalTile normalTile;
    public BrokenTile brokenTile;
    public OnceTile onceTile;
    public SpringTile springTile;
    public MoveHorTile moveHorTile;
    public MoveVerTile moveVerTile;

    [Space(10), Header("Item")]

    public ItemBase itemPrefab;
    public string itemParent;
    public float hatFlyTime = 1.5f;
    public float rocketFlyTime = 3f;
    public float hatWeight = 0.1f;
    public float rocketWeight = 0.05f;

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