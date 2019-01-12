using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public List<MoneyBase> ShowMoneyList { get; private set; }
    public ObjectPool<MoneyBase> MoneyPool { get; private set; }

    private GameData gameData;

    public MoneyManager()
    {
        gameData = GameManager.GameData;
        ShowMoneyList = new List<MoneyBase>();
        MoneyPool = new ObjectPool<MoneyBase>(gameData.moneyPrefab, 10, gameData.moneyParent);
    }
}
