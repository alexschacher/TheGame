using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField] private WorldController worldController;

    void Awake()
    {
        Resource.LoadResources();
        worldController.Init();
    }
}