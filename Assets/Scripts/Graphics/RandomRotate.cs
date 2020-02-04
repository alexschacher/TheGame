using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    [SerializeField]
    private Vector2 rotateRange = new Vector2(0, 15);

    void Start()
    {
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + Random.Range(rotateRange.x, rotateRange.y),
            transform.eulerAngles.z
            );
    }
}
