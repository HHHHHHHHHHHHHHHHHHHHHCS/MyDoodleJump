using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
/// 物品管理器
/// </summary>
public class ItemManager
{
    public List<ItemBase> ShowItemList { get; private set; }
    public ObjectPool<ItemBase> ItemPool { get; private set; }

    private GameData gameData;

    public ItemManager OnInit()
    {
        gameData = MainGameManager.GameData;
        ShowItemList = new List<ItemBase>();
        ItemPool = new ObjectPool<ItemBase>(gameData.itemPrefab, 0, gameData.itemParent);
        return this;
    }


    /// <summary>
    /// 检查是否能创建
    /// </summary>
    public ItemType CheckNeedCreate()
    {
        var rd = Random.Range(0, gameData.AllItemWeight);

        if (rd <= 1)
        {
            return ItemType.None;
        }

        if (rd <= gameData.hatWeight)
        {
            return ItemType.Hat;
        }

        if (rd <= gameData.rocketWeight)
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
                tile.IsBind = true;
                tile.BindItem = temp;
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
        item.BindTile.BindItem = null;
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