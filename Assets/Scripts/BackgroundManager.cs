using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager
{
    private float offsetY = 19.2f;

    private Transform[] brackgrounds;
    private float highPosY = int.MinValue;

    public void OnStart()
    {
        var bgs = GameObject.Find("Backgrounds").transform;
        brackgrounds = new Transform[bgs.childCount];
        for (int i = 0; i < bgs.childCount; i++)
        {
            brackgrounds[i] = bgs.GetChild(i);
            if (brackgrounds[i].position.y > highPosY)
            {
                highPosY = brackgrounds[i].position.y;
            }
        }
    }

    public void OnUpdate(float nowY)
    {
        nowY -= offsetY;
        foreach (var item in brackgrounds)
        {
            if (item.position.y <= nowY)
            {
                var pos = item.position;
                highPosY += offsetY;
                pos.y = highPosY;
                item.position = pos;
            }
        }
    }
}