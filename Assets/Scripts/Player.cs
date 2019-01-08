using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    private Vector3 leftDir = new Vector3(-1, 1, 1),
        rightDir = new Vector3(1, 1, 1);
    private float leftBorder, rightBorder;
    private Rigidbody2D rigi;
    private Collider2D col2d;

    protected override void OnAwake()
    {
        rigi = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        var mainCam = Camera.main;
        leftBorder = mainCam.ViewportToWorldPoint(Vector3.zero).x;
        rightBorder = mainCam.ViewportToWorldPoint(Vector3.right).x;
        Pause();
    }

    private void Update()
    {
        if(GameManager.Instance.GameState != GameState.Running)
        {
            return;
        }

        if (CheckDie())
        {
            Die();
        }

        Vector3 acc = Vector3.zero;
        var ori = transform.localPosition;
        if (Input.GetKey(KeyCode.A))
        {
            acc.x -= 0.1f;
            transform.localScale = leftDir;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            acc.x += 0.1f;
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
        if (GameManager.Instance.GameState != GameState.Running)
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
    public void Fly(float Time)
    {
        rigi.velocity = Vector2.zero;
        rigi.isKinematic = true;
        col2d.enabled = false;
    }

    /// <summary>
    /// 检查是否死亡
    /// </summary>
    public bool CheckDie()
    {
        if (transform.position.y < GameManager.Instance.RecoverY)
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
        GameManager.Instance.PlayerDie();
    }


}
