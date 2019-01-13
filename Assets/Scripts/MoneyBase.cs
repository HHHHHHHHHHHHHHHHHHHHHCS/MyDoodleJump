using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBase : MonoBehaviour
{
    public TileBase BindTile { get; private set; }

    private int moneyValue;
    private MoneyType moneyType;

    private GameObject coin;

    private void Awake()
    {
        coin = transform.Find("Coin").gameObject;
    }

    public void Init(TileBase tile,MoneyType type)
    {
        HideAll();
        BindTile = tile;
        moneyType = type;
        moneyValue = 0;
        switch (moneyType)
        {
            case MoneyType.Coin:
                coin.SetActive(true);
                moneyValue = MainGameManager.Instance.gameData.coinValue;
                break;
        }

        OnUpdate();
        gameObject.SetActive(true);
    }


    public void HideAll()
    {
        coin.SetActive(false);
    }

    public void OnUpdate()
    {
        transform.position = BindTile.CenterUpPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player))
        {
            var manager = MainGameManager.Instance;
            manager.MoneyManager.RecoveryMoney(this);
            manager.GetMoney(moneyValue);
        }
    }
}
