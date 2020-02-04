using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private float xAngle = 15f;
    
    void LateUpdate()
    {
        SetRotationToCamera();
    }

    private void SetRotationToCamera()
    {
        // Set rotation to camera rotation
        transform.forward = Camera.main.transform.forward;

        // Reset x rotation
        transform.eulerAngles = new Vector3(xAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
