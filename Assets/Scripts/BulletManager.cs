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

        for (int i = ShowBulletList.Count - 1; i >= 0; i--)
        {
            ShowBulletList[i].OnUpdate();
        }
    }

    public bool SpawnBullet(Vector3 spawnPoint, Vector3 touchPos)
    {
        if (bulletNextTimer > 0)
        {
            return false;
        }

        touchPos =mainCam.ScreenToWorldPoint(touchPos);
        touchPos.z = spawnPoint.z;
        bulletNextTimer = gameData.bulletNextTime;
        var temp = BulletPool.Get();
        var dir = (touchPos - spawnPoint).normalized;
        temp.OnInit(spawnPoint, dir);
        ShowBulletList.Add(temp);
        return true;
    }

    public void RecoveryBullet(BulletBase bullet)
    {
        ShowBulletList.Remove(bullet);
        BulletPool.Put(bullet);
    }
}