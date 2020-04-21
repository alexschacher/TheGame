using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleCommand : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] private float timeScale = 1f;
    [SerializeField] private bool showDebug = false;
    [SerializeField] private bool invincible = false;

    void Update()
    {
        Time.timeScale = this.timeScale;

        //GetComponentInChildren<Canvas>().enabled = showDebug;
    }

    public bool IsDebugVisible()
    {
        return showDebug;
    }

    public bool IsInvincible()
    {
        return invincible;
    }
}
