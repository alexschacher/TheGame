using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteFlip : MonoBehaviour
{
    void Awake()
    {
        if (Random.Range(0f, 1f) > 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
