using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float damoTime = 0.5f;

    private Camera mainCam;
    private Vector3 velocity, lastPos;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (target)
        {
            var pos = mainCam.WorldToViewportPoint(target.position);
            var delta = target.position - mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, pos.z));
            Vector3 destination = transform.position;
            destination.y += delta.y;
            if (destination.y > lastPos.y)
            {
                lastPos = destination;
            }
            if (lastPos.y > transform.position.y)
            {
                transform.position = Vector3.SmoothDamp(lastPos, destination, ref velocity, damoTime);
            }

        }
    }
}
