using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTilt : MonoBehaviour
{
    [SerializeField]
    private float tiltRange = 5f;

    void Start()
    {
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x + Random.Range(-tiltRange, tiltRange),
            transform.eulerAngles.y,
            transform.eulerAngles.z + Random.Range(-tiltRange, tiltRange)
            );
    }
}
