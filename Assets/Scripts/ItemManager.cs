using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public List<TileBase> ShowItemList { get; private set; }
    public ObjectPool<ItemBase> ItemPool { get; private set; }

    private GameData gameData;

    public ItemManager()
    {
        gameData = GameManager.GameData;
        ShowItemList = new List<TileBase>();
        ItemPool = new ObjectPool<ItemBase>(gameData.itemPrefab, 10, gameData.itemParent);
    }

    /// <summary>
    /// 检查是否能创建
    /// </summary>
    public ItemType CheckNeedCreate()
    {
        var rd = Random.Range(0, gameData.AllItemWeight);
        if (rd < 1)
        {
            return ItemType.None;
        }

        rd -= 1;
        if (rd < gameData.hatWeight)
        {
            return ItemType.Hat;
        }

        rd -= gameData.hatWeight;
        if (rd < gameData.rocketWeight)
        {
            return ItemType.Rocket;
        }

        return ItemType.None;
    }

    public ItemBase SpawnItem(TileBase tile)
    {
        var temp =ItemPool.Get();

        //TODO:offset

        return temp;
    }
}
