using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private float variation;
    [SerializeField]
    private int rate = 1;

    [SerializeField]
    private Light lightSource;

    private int count;
    private float originalIntensity;

    private void Start()
    {
        Light lightSource = GetComponent<Light>();
        originalIntensity = lightSource.intensity;
    }

    void Update()
    {
        count++;
        if (count > rate)
        {
            count = 0;
            lightSource.intensity = Random.Range(originalIntensity - variation, originalIntensity + variation);
        }
    }
}
