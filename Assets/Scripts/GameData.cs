using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance;

    public static GameData Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.Find("GameData").GetComponent<GameData>();

            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }


    public Sprite[] titleSprite;

    private void Awake()
    {
        if (!instance)
        {
            Instance = this;
        }
    }
}

public class Tags
{
    public const string Player = "Player";
    public const string Platform = "Platform";
}