using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    private Transform parent;
    private SpriteRenderer rend;

    private void Start()
    {
        parent = GetComponentInParent<Transform>();
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (parent.localRotation.eulerAngles.y > 180 && parent.localRotation.eulerAngles.y < 360)
        {
            rend.flipX = false;
        }
        else if (parent.transform.localRotation.eulerAngles.y > 0 && parent.localRotation.eulerAngles.y < 180)
        {
            rend.flipX = true;
        }
    }
}