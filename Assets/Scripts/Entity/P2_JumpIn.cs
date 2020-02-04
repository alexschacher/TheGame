using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2_JumpIn : MonoBehaviour
{
    [SerializeField]
    private GameObject p2_prefab;

    private bool spawned = false;

    void Update()
    {
        if (Input.GetButtonDown("Jump_P2") && !spawned)
        {
            spawned = true;
            FollowTarget followTarget = GetComponentInChildren<FollowTarget>();
            GameObject spawnedObj = Instantiate(p2_prefab,new Vector3(
                followTarget.transform.position.x,
                followTarget.transform.position.y + 1.5f,
                followTarget.transform.position.z), Quaternion.identity);
            followTarget.SetTarget2(spawnedObj.transform);
        }
    }
}
