using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate90 : MonoBehaviour
{
    void Start()
    {
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + (90 * Random.Range(0, 3)),
            transform.eulerAngles.z
            );
    }
}
