using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSprite : MonoBehaviour
{
    public enum AdjustEnum
    {
        None,
        X,
        Y,
        XY,
    }


    private static float screenScale, aspectScale;
    private static Camera mainCam;

    public AdjustEnum adjustEnum;

    private void Awake()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
            screenScale = (float)Screen.height / Screen.width;
            aspectScale = mainCam.orthographicSize * 2 / Screen.height * Screen.width;
        }
        switch (adjustEnum)
        {
            case AdjustEnum.None:
                break;
            case AdjustEnum.X:
                ResizeX();
                break;
            case AdjustEnum.Y:
                ResizeY();
                break;
            case AdjustEnum.XY:
                ResizeXY();
                break;
            default:
                break;
        }
        Destroy(this);
    }

    public void ResizeX()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr)
        {
            var sp = sr.sprite;
            var scale = transform.localScale;

            scale.x = aspectScale / sr.bounds.size.x;
            transform.localScale = scale;
        }
    }

    public void ResizeY()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr)
        {
            var sp = sr.sprite;
            var scale = transform.localScale;
            scale.y = aspectScale / sr.bounds.size.y * Screen.height / Screen.width;
            transform.localScale = scale;
        }
    }

    public void ResizeXY()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr)
        {
            var sp = sr.sprite;
            var scale = transform.localScale;
            scale.x = aspectScale / sr.bounds.size.x;
            scale.y = aspectScale / sr.bounds.size.y * screenScale;
            transform.localScale = scale;
        }
    }
}
