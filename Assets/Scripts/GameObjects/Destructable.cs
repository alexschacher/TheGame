using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public void Damage(float damage)
    {
        GameObject.Destroy(this.transform.gameObject);
    }
}
