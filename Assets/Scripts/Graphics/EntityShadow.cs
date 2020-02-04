using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityShadow : MonoBehaviour
{
    private void Update()
    {
        // Just to force rotation to stay at 90
        transform.SetPositionAndRotation(new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z),
            Quaternion.identity);
        transform.Rotate(new Vector3(90, 0, 0));
    }

    void FixedUpdate()
    {
        transform.SetPositionAndRotation(new Vector3(
            transform.parent.position.x,
            transform.parent.position.y + 0.02f,
            transform.parent.position.z),
            Quaternion.identity);

        transform.Rotate(new Vector3(90, 0, 0));

        RaycastHit rayCastHit;
        Physics.Raycast(transform.parent.position - new Vector3(0, 0.1f, 0), -Vector3.up, out rayCastHit, 20f, LayerMask.GetMask("Environment"));

        //Debug.Log(rayCastHit.collider.transform.name);

        transform.SetPositionAndRotation(new Vector3(
            transform.parent.position.x,
            transform.parent.position.y - rayCastHit.distance + 0.02f,
            transform.parent.position.z),
            transform.rotation);
    }
}
