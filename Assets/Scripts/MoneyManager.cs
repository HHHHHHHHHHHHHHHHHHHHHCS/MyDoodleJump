using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 金钱管理
/// </summary>
public class MoneyManager
{
    public List<MoneyBase> ShowMoneyList { get; private set; }
    public ObjectPool<MoneyBase> MoneyPool { get; private set; }

    private readonly GameData gameData;

    public MoneyManager()
    {
        gameData = MainGameManager.GameData;
        ShowMoneyList = new List<MoneyBase>();
        MoneyPool = new ObjectPool<MoneyBase>(gameData.moneyPrefab, 0, gameData.moneyParent);
    }


    /// <summary>
    /// 更新位置
    /// </summary>
    public void OnUpdate()
    {
        foreach (var item in ShowMoneyList)
        {
            item.OnUpdate();
        }
    }

    /// <summary>
    /// 是否可以创建金钱
    /// </summary>
    /// <returns></returns>
    public MoneyType CheckNeedCreate()
    {
        var rd = Random.Range(0, gameData.AllMoenyWeight);
        if (rd <= 1)
        {
            return MoneyType.None;
        }

        rd -= 1;
        if (rd <= gameData.coinWeight)
        {
            return MoneyType.Coin;
        }

        return MoneyType.None;
    }


    /// <summary>
    /// 创建金钱
    /// </summary>
    /// <param name="tile"></param>
    public void SpawnMoney(TileBase tile)
    {
        if (!tile.IsBind)
        {
            var type = CheckNeedCreate();
            if (type != MoneyType.None)
            {
                var temp = MoneyPool.Get();
                tile.IsBind = true;
                tile.BindMoney = temp;
                temp.Init(tile, type);
                ShowMoneyList.Add(temp);
            }
        }
    }


    /// <summary>
    /// 回收金钱
    /// </summary>
    public void RecoveryMoney(MoneyBase money)
    {
        money.BindTile.BindMoney = null;
        money.BindTile.IsBind = false;
        money.HideAll();
        ShowMoneyList.Remove(money);
        MoneyPool.Put(money);
    }

    /// <summary>
    /// 回收绑定跳板的金钱
    /// </summary>
    public void RecoveryBindMoney(TileBase tile)
    {
        if (tile.BindMoney)
        {
            RecoveryMoney(tile.BindMoney);
        }
    }
}