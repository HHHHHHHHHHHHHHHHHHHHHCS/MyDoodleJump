using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 leftDir = new Vector3(-1, 1, 1),
        rightDir = new Vector3(1, 1, 1);
    private float leftBorder, rightBorder;
    private Rigidbody2D rigi;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        var mainCam = Camera.main;
        leftBorder = mainCam.ViewportToWorldPoint(Vector3.zero).x;
        rightBorder = mainCam.ViewportToWorldPoint(Vector3.right).x;
    }

    private void Update()
    {
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
            , ori + acc, 10*Time.deltaTime);
        diff.y = ori.y;
        diff.z = ori.z;

        if(diff.x<leftBorder)
        {
            diff.x = rightBorder;
        }
        else if(diff.x>rightBorder)
        {
            diff.x = leftBorder;
        }

        transform.localPosition = diff;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            Jump(1);
        }
    }

    public void Jump(float speedScale)
    {
        rigi.velocity = Vector2.zero;
        //Impulse瞬间的力,不再采用系统的帧频间隔
        rigi.AddForce(new Vector2(0, 12 * speedScale), ForceMode2D.Impulse);
    }
}
