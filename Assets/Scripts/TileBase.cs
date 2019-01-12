using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    private const float downSpeed = 20;

    [SerializeField]
    private TileType tileType;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D col2D;
    private Coroutine coroutine;
    private bool isDown;
    private int moveDir;//-1 左下, 1 右上
    private Vector2 startPos;

    public bool IsBind{ get; set; }

    public Vector3 CenterUpPos
    {
        get
        {
            Vector3 v3 = transform.position;
            v3.y += spriteRenderer.sprite.bounds.size.y/2;
            return v3;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<BoxCollider2D>();
    }

    public void OnUpdate()
    {
        switch (tileType)
        {
            case TileType.MoveHorTile:
                {


                    var moveHorTile = GameManager.GameData.moveHorTile;
                    Vector2 newPos = Vector2.zero;
                    newPos.x = moveDir * moveHorTile.speed * Time.deltaTime;
                    transform.Translate(newPos);
                    if (transform.position.x - startPos.x >= moveHorTile.distance)
                    {
                        moveDir = -1;
                    }
                    else if (transform.position.x - startPos.x <= -moveHorTile.distance)
                    {
                        moveDir = 1;
                    }
                    break;
                }
            case TileType.MoveVerTile:
                {
                    var moveVerTile = GameManager.GameData.moveVerTile;
                    Vector2 newPos = Vector2.zero;
                    newPos.y = moveDir * moveVerTile.speed * Time.deltaTime;
                    transform.Translate(newPos);
                    if (transform.position.y - startPos.y >= moveVerTile.distance)
                    {
                        moveDir = -1;
                    }
                    else if (transform.position.y - startPos.y <= -moveVerTile.distance)
                    {
                        moveDir = 1;
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(TileType type, Vector2 pos)
    {
        tileType = type;
        startPos = pos;
        if (tileType == TileType.MoveHorTile || tileType == TileType.MoveVerTile)
        {
            moveDir = Random.value < 0.5f ? -1 : 1;
        }
        transform.position = pos;
        Active();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 回收
    /// </summary>
    public void Recovery()
    {
        gameObject.SetActive(false);
        StopFallDown();
    }


    /// <summary>
    /// 重新激活方块
    /// </summary>
    public void Active()
    {
        col2D.enabled = true;
        var sprites = GameManager.GameData.titleSprite;
        int type = (int)tileType;
        if (type < sprites.Length)
        {
            spriteRenderer.sprite = sprites[type];
        }
        else
        {
            Debug.Log("Enum大于图片长度");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player)
            && collision.GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            var player = collision.GetComponent<Player>();
            float jumpScale = 1;
            switch (tileType)
            {
                case TileType.NormalTile:
                    break;
                case TileType.BrokenTile:
                    FallDown();
                    break;
                case TileType.OnceTile:
                    FallDown();
                    break;
                case TileType.SpringTile:
                    jumpScale = 1.5f;
                    break;
                case TileType.MoveHorTile:
                    break;
                case TileType.MoveVerTile:
                    break;
                default:
                    break;
            }
            player.Jump(jumpScale);
        }
    }

    /// <summary>
    /// 掉落用
    /// </summary>
    public void FallDown()
    {
        StopFallDown();
        col2D.enabled = false;
        isDown = true;
        coroutine = StartCoroutine(Down());
    }

    /// <summary>
    /// 暂停下落
    /// </summary>
    public void StopFallDown()
    {
        if (coroutine != null)
        {
            isDown = false;
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    /// <summary>
    /// 下降
    /// </summary>
    private IEnumerator Down()
    {
        while (isDown)
        {
            if (GameManager.Instance.tileManager.NeedRecoveryTile(this))
            {
                StopFallDown();
                yield break;
            }
            var pos = transform.localPosition;
            pos.y -= downSpeed * Time.deltaTime;
            transform.localPosition = pos;
            yield return null;
        }
    }


}
