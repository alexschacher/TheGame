using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStretch : MonoBehaviour
{
    [SerializeField]
    private Vector3 stretchRange = new Vector3(0f, 0f, 0f);

    void Start()
    {
        transform.localScale = new Vector3(
            transform.localScale.x + Random.Range(0, stretchRange.x),
            transform.localScale.y + Random.Range(0, stretchRange.y),
            transform.localScale.z + Random.Range(0, stretchRange.z)
            );
    }
}
