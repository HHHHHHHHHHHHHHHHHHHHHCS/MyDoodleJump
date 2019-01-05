using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Tile = 0,
    Item,
    Coin,
    Enemy,
    Bullet,
}

public class GameData : MonoSingleton<GameData>
{
    public const float xMinBorder = -4.5f, xMaxBorder = 4.5f;
    public const float startTilePosY = -4;

    public Sprite[] titleSprite;
    public NormalTile normalTile;
    public BrokenTile brokenTile;
    public OnceTile onceTile;
    public SpringTile springTile;
    public MoveHorTile moveHorTile;
    public MoveVerTile moveVerTile;

    private float sumAllWeight;

    public float SumAllWeight { get => sumAllWeight; }

    static GameData()
    {
        singletonPath = "GameData";
    }

    protected override void OnAwake()
    {
        sumAllWeight = normalTile.weight + brokenTile.weight + onceTile.weight
            + springTile.weight + moveHorTile.weight + moveVerTile.weight;
    }


}

public class Tags
{
    public const string Player = "Player";
    public const string Platform = "Platform";
}