using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public List<ItemBase> ShowItemList { get; private set; }
    public ObjectPool<ItemBase> ItemPool { get; private set; }

    private readonly GameData gameData;

    public ItemManager()
    {
        gameData = MainGameManager.GameData;
        ShowItemList = new List<ItemBase>();
        ItemPool = new ObjectPool<ItemBase>(gameData.itemPrefab, 0, gameData.itemParent);
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
        if (!tile.IsBind)
        {
            var type = CheckNeedCreate();
            if (type != ItemType.None)
            {
                var temp = ItemPool.Get();
                temp.Init(tile, type);
                ShowItemList.Add(temp);
            }
        }
    }

    /// <summary>
    /// 回收物品
    /// </summary>
    public void RecoveryItem(ItemBase item)
    {
        item.BindTile.IsBind = false;
        item.HideAll();
        ShowItemList.Remove(item);
        ItemPool.Put(item);
    }

    /// <summary>
    /// 回收绑定跳板的物品
    /// </summary>
    public void RecoveryBindItem(TileBase tile)
    {
        if (tile.BindItem)
        {
            RecoveryItem(tile.BindItem);
        }
    }
}