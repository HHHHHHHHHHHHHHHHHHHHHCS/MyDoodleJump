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
    public Sprite[] titleSprite;
    public NormalTile normalTile;
    public BrokenTile brokenTile;
    public OnceTile onceTile;
    public SpringTile springTile;
    public MoveHorTile moveHorTile;
    public MoveVerTile moveVerTile;


    static GameData()
    {
        singletonPath = "GameData";
    }
}

public class Tags
{
    public const string Player = "Player";
    public const string Platform = "Platform";
}