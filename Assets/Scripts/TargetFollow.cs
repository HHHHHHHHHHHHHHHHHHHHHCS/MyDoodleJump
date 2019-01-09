using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float dampTime = 0.5f;

    private Camera mainCam;
    private Vector3 velocity, destination;
    private float lastTargetY;

    private void Awake()
    {
        mainCam = Camera.main;
        destination = Vector3.one * float.MinValue;
        lastTargetY = float.MinValue;
    }

    private void Update()
    {
        if (target && target.position.y > lastTargetY)
        {
            lastTargetY = target.position.y;
            var pos = mainCam.WorldToViewportPoint(target.position);
            var delta = target.position - mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, pos.z));

            if (delta.y > 0)
            {
                destination = transform.position;
                destination.y += delta.y;
            }
        }
        if (transform.position.y < destination.y)
        {
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            GameManager.Instance.DoRecovery(transform.position.y);
        }
    }
}
