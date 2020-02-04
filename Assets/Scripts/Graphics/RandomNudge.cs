using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNudge : MonoBehaviour
{
    [SerializeField]
    private Vector3 nudgeRange = new Vector3(0.05f, 0f, 0.05f);

    void Start()
    {
        transform.position = new Vector3(
            transform.position.x + Random.Range(-nudgeRange.x, nudgeRange.x),
            transform.position.y + Random.Range(-nudgeRange.y, nudgeRange.y),
            transform.position.z + Random.Range(-nudgeRange.z, nudgeRange.z)
            );
    }
}
