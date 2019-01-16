using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    public List<BulletBase> ShowBulletList { get; private set; }
    public ObjectPool<BulletBase> BulletPool { get; private set; }

    private GameData gameData;
    private float bulletNextTimer;
    private Camera mainCam;

    public BulletManager OnInit()
    {
        gameData = MainGameManager.GameData;
        mainCam = Camera.main;
        ShowBulletList = new List<BulletBase>();
        BulletPool = new ObjectPool<BulletBase>(gameData.bulletPrefab, 0, gameData.bulletParent);
        return this;
    }

    public void OnUpdate()
    {
        if (bulletNextTimer > 0)
        {
            bulletNextTimer -= Time.deltaTime;
        }
    }

    public void SpawnBullet(Vector3 touchPos)
    {
        var pos = mainCam.WorldToViewportPoint(Player.Instance.transform.position);
    }
}