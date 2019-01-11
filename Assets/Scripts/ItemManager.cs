using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public List<ItemBase> ShowItemList { get; private set; }
    public ObjectPool<ItemBase> ItemPool { get; private set; }

    private GameData gameData;

    public ItemManager()
    {
        gameData = GameManager.GameData;
        ShowItemList = new List<ItemBase>();
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

    /// <summary>
    /// 更新位置
    /// </summary>
    public void OnUpdate()
    {
        foreach (var item in ShowItemList)
        {
            item.OnUpdate();
        }
    }

    /// <summary>
    /// 创建
    /// </summary>
    public void SpawnItem(TileBase tile)
    {
        var type = CheckNeedCreate();
        if (type != ItemType.None)
        {
            var temp = ItemPool.Get();
            temp.Init(tile, type);
            ShowItemList.Add(temp);
        }
    }

    /// <summary>
    /// 判断是否需要回收物品
    /// </summary>
    public bool NeedRecoveryItem(ItemBase tile,float checkPosY)
    {
        if (tile.transform.position.y > checkPosY)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 回收跳板用
    /// </summary>
    public void RecoveryTile(float checkPosY)
    {
        int removeIndex = -1;
        int count = ShowItemList.Count - 1;

        for (int i = 0; i < ShowItemList.Count; i++)
        {//标记是否可回收
            if (!NeedRecoveryItem(ShowItemList[i], checkPosY))
            {
                removeIndex = i - 1;
                break;
            }
            else if (i == count)
            {
                removeIndex = count;
            }
        }

        for (int i = 0; i <= removeIndex; i++)
        {//回收
            var tempTile = ShowItemList[0];
            RecoveryItem(tempTile);
        }

    }

    public void RecoveryItem(ItemBase item)
    {
        item.HideAll();
        ShowItemList.Remove(item);
        ItemPool.Put(item);
    }
}
