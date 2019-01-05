using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    private const float downSpeed = 20;

    [SerializeField]
    private TileType titleType;


    private SpriteRenderer spriteRenderer;
    private BoxCollider2D col2D;
    private Coroutine coroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<BoxCollider2D>();
    }

    public void Init(TileType type ,Vector2 pos)
    {
        titleType = type;
        transform.position = pos;
        Active();
        gameObject.SetActive(true);
    }


    public void Active()
    {
        col2D.enabled = true;
        var sprites = GameData.Instance.titleSprite;
        int type = (int)titleType;
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
            switch (titleType)
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

    private void FallDown()
    {
        col2D.enabled = false;
        coroutine = StartCoroutine(Down());
    }

    private IEnumerator Down()
    {
        while (true)
        {
            var pos = transform.localPosition;
            pos.y -= downSpeed * Time.deltaTime;
            transform.localPosition = pos;
            yield return null;
        }
    }
}
