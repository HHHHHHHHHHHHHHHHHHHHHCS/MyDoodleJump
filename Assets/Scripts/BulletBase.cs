using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    private Vector3 moveDir;
    private float destroyTimer;

    public void OnInit(Vector3 dir)
    {
        GameData data = MainGameManager.GameData;
        moveDir = dir * data.bulletMoveSpeed;
        destroyTimer = data.bulletDestroyTime;
    }

    public void OnUpdate()
    {
        destroyTimer -= Time.deltaTime;
        if (destroyTimer<=0)
        {
            //回收自己
        }
        else
        {
            transform.Translate(moveDir);
        }
    }
}