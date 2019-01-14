using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    private readonly Vector3 leftDir = new Vector3(-1, 1, 1),
        rightDir = new Vector3(1, 1, 1);

    private float leftBorder, rightBorder;
    private Rigidbody2D rigi;
    private Collider2D col2d;
    private GameObject hat_Used, rocket_Used;
    private bool isFly;

    protected override void OnAwake()
    {
        rigi = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        hat_Used = transform.Find("Hat_Used").gameObject;
        rocket_Used = transform.Find("Rocket_Used").gameObject;
        var mainCam = Camera.main;
        leftBorder = mainCam.ViewportToWorldPoint(Vector3.zero).x;
        rightBorder = mainCam.ViewportToWorldPoint(Vector3.right).x;
        Pause();
    }

    private void Update()
    {
        if (MainGameManager.Instance.GameState != GameState.Running)
        {
            return;
        }

        if (CheckDie())
        {
            Die();
        }

        if (isFly)
        {
            rigi.velocity = Vector2.zero;
            transform.Translate(MainGameManager.GameData.playerFlySpeed
                                * Time.deltaTime * Vector3.up);
        }

        Vector3 acc = Vector3.zero;
        var ori = transform.localPosition;
        if (Input.GetKey(KeyCode.A))
        {
            acc.x -= MainGameManager.GameData.playerHorSpeed;
            transform.localScale = leftDir;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            acc.x += MainGameManager.GameData.playerHorSpeed;
            transform.localScale = rightDir;
        }

        var diff = Vector3.MoveTowards(ori
            , ori + acc, 10 * Time.deltaTime);
        diff.y = ori.y;
        diff.z = ori.z;

        if (diff.x < leftBorder)
        {
            diff.x = rightBorder;
        }
        else if (diff.x > rightBorder)
        {
            diff.x = leftBorder;
        }

        transform.localPosition = diff;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MainGameManager.Instance.GameState != GameState.Running)
        {
            return;
        }

        if (collision.CompareTag(Tags.Platform))
        {
            Jump(1);
        }
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    public void Jump(float speedScale)
    {
        rigi.velocity = Vector2.zero;
        //Impulse瞬间的力,不再采用系统的帧频间隔
        rigi.AddForce(new Vector2(0, 12 * speedScale), ForceMode2D.Impulse);
    }

    /// <summary>
    /// 飞行
    /// </summary>
    public void Fly(ItemType itemType, float time)
    {
        rigi.velocity = Vector2.zero;
        rigi.isKinematic = true;
        col2d.enabled = false;
        isFly = true;
        switch (itemType)
        {
            case ItemType.Hat:
            {
                hat_Used.SetActive(true);
                break;
            }
            case ItemType.Rocket:
            {
                rocket_Used.SetActive(true);
                break;
            }
        }

        StartCoroutine(StopFly(itemType, time));
    }

    private IEnumerator StopFly(ItemType itemType, float time)
    {
        yield return new WaitForSeconds(time);
        rigi.isKinematic = false;
        col2d.enabled = true;
        isFly = false;
        switch (itemType)
        {
            case ItemType.Hat:
            {
                hat_Used.SetActive(false);
                break;
            }
            case ItemType.Rocket:
            {
                rocket_Used.SetActive(false);
                break;
            }
        }
    }

    /// <summary>
    /// 检查是否死亡
    /// </summary>
    public bool CheckDie()
    {
        if (transform.position.y < MainGameManager.Instance.RecoverY)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 继续
    /// </summary>
    public void Resume()
    {
        rigi.gravityScale = 1;
        col2d.enabled = true;
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        rigi.gravityScale = 0;
        col2d.enabled = false;
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Die()
    {
        rigi.gravityScale = 0;
        col2d.enabled = false;
        MainGameManager.Instance.PlayerDie();
    }
}