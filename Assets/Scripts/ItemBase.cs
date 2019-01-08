using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    private ItemType itemType;

    private GameObject hat, rocket;
    private float flyTime;

    private void Awake()
    {
        hat = transform.Find("Hat").gameObject;
        rocket = transform.Find("Rocket").gameObject;
    }

    public void Init()
    {
        HideAll();
        flyTime = 0;
        gameObject.SetActive(true);
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
    }

    public void HideAll()
    {
        hat.SetActive(false);
        rocket.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player))
        {
            GameManager.Instance.ItemPool.Put(this);
            Player.Instance.Fly(flyTime);
        }
    }
}
