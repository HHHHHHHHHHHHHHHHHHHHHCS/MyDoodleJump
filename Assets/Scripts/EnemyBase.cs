using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private GameObject enemy1, enemy2, enemy3;

    private EnemyType enemyType;
    private Vector3 moveSpeed;
    private int direction; //-1左边 0 不动 1右边

    public void Awake()
    {
        enemy1 = transform.Find("Enemy1").gameObject;
        enemy2 = transform.Find("Enemy2").gameObject;
        enemy3 = transform.Find("Enemy3").gameObject;
    }

    public void Init(EnemyType type, float posY)
    {
        HideAll();
        enemyType = type;
        var x = Random.Range(GameData.xMinBorder, GameData.xMaxBorder);
        var y = posY + Random.Range(MainGameManager.GameData.enemyMinHeight, MainGameManager.GameData.enemyMaxHeight);
        transform.position = new Vector3(x, y, 0);
        switch (type)
        {
            case EnemyType.Enemy1:
                enemy1.SetActive(true);
                direction = 0;
                break;
            case EnemyType.Enemy2:
                enemy2.SetActive(true);
                direction = Random.value < 0.5f ? -1 : 1;
                moveSpeed = Vector3.right * MainGameManager.GameData.enemy1Weight;
                break;
            case EnemyType.Enemy3:
                enemy3.SetActive(true);
                direction = Random.value < 0.5f ? -1 : 1;
                moveSpeed = Vector3.right * MainGameManager.GameData.enemy2Weight;
                break;
        }
        gameObject.SetActive(true);
    }

    public void OnUpdate()
    {
        var manager = MainGameManager.Instance;
        if (transform.position.y < manager.RecoverY)
        {
            manager.EnemyManager.RecoveryEnemy(this);
        }

        if (direction != 0)
        {
            transform.Translate(moveSpeed * Time.deltaTime * direction);
            if (transform.position.x <= GameData.xSafeMinBorder)
            {
                if (enemyType == EnemyType.Enemy2)
                {
                    direction *= -1;
                }
                else if (enemyType == EnemyType.Enemy3)
                {
                    Vector3 pos = transform.position;
                    pos.x = GameData.xSafeMaxBorder;
                    transform.position = pos;
                }
            }
            else if (transform.position.x >= GameData.xSafeMaxBorder)
            {
                if (enemyType == EnemyType.Enemy2)
                {
                    direction *= -1;
                }
                else if (enemyType == EnemyType.Enemy3)
                {
                    Vector3 pos = transform.position;
                    pos.x = GameData.xSafeMinBorder;
                    transform.position = pos;
                }
            }
        }
    }

    public void HideAll()
    {
        enemy1.SetActive(false);
        enemy2.SetActive(false);
        enemy3.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var manager = MainGameManager.Instance;
        if (manager.GameState != GameState.Running)
        {
            return;
        }

        if (collision.CompareTag(Tags.Bullet))
        {
            manager.BulletManager.RecoveryBullet(collision.GetComponent<BulletBase>());
            manager.EnemyManager.RecoveryEnemy(this);
        }

        if (collision.CompareTag(Tags.Player))
        {
            manager.PlayerDie();
        }


    }
}