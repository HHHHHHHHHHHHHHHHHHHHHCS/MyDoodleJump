using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    private Vector3 moveDir;
    private float destroyTimer;

    public void OnInit(Vector3 spawnPoint,Vector3 dir)
    {
        transform.position = spawnPoint;
        GameData data = MainGameManager.GameData;
        moveDir = dir * data.bulletMoveSpeed;
        destroyTimer = data.bulletDestroyTime;
        gameObject.SetActive(true);
    }

    public void OnUpdate()
    {
        destroyTimer -= Time.deltaTime;
        if (destroyTimer<=0)
        {
            MainGameManager.Instance.BulletManager.RecoveryBullet(this);
        }
        else
        {
            transform.Translate(moveDir*Time.deltaTime);
        }
    }
}