using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    private ItemType itemType;

    private GameObject hat, rocket;
    private float flyTime;

    /// <summary>
    /// 绑定的跳板
    /// </summary>
    public TileBase BindTile { get; private set; }


    private void Awake()
    {
        hat = transform.Find("Hat").gameObject;
        rocket = transform.Find("Rocket").gameObject;
    }

    public void Init(TileBase tile,ItemType type)
    {
        HideAll();
        flyTime = 0;
        itemType = type;
        BindTile = tile;
  
        switch (itemType)
        {
            case ItemType.Hat:
                hat.SetActive(true);
                flyTime = GameManager.GameData.hatFlyTime;
                break;
            case ItemType.Rocket:
                rocket.SetActive(true);
                flyTime = GameManager.GameData.rocketFlyTime;
                break;
            default:
                break;
        }

        OnUpdate();
        gameObject.SetActive(true);
    }

    public void OnUpdate()
    {
        transform.position = BindTile.CenterUpPos;
    }

    /// <summary>
    /// 全部隐藏
    /// </summary>
    public void HideAll()
    {
        hat.SetActive(false);
        rocket.SetActive(false);
    }

    /// <summary>
    /// 玩家进入
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player))
        {
            GameManager.Instance.itemManager.RecoveryItem(this);
            Player.Instance.Fly(itemType,flyTime);
        }
    }
}
