using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform target2;
    [SerializeField] private float smoothSpeed = 0.25f;
    [SerializeField] private Vector3 offset = Vector3.zero;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (target & !target2)
        {
            transform.position = Vector3.SmoothDamp(
            transform.position,
            target.position + offset,
            ref velocity,
            smoothSpeed);
        }
        else if (!target & target2)
        {
            transform.position = Vector3.SmoothDamp(
            transform.position,
            target2.position + offset,
            ref velocity,
            smoothSpeed);
        }
        else if (target & target2)
        {
            transform.position = Vector3.SmoothDamp(
            transform.position,
            ((target.position + target2.position) / 2) + offset,
            ref velocity,
            smoothSpeed);
        }
    }

    public void SetTarget(Transform transform)
    {
        target = transform;
    }

    public void SetTarget2(Transform transform)
    {
        target2 = transform;
    }
}