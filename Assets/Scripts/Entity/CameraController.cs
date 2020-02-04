using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotationSpeed = 5f;
    private float zoomSpeed = 2f;
    private float angleSnap = 45f;

    private Vector3 zoomPosition0 = new Vector3(0f, -2.4f, -24f);
    private Vector3 zoomPosition1 = new Vector3(0f, -3.6f, -19f);
    private Vector3 zoomPosition2 = new Vector3(0f, -4.8f, -14f);
    private int zoomPosition = 1;
    private bool canZoom = true;

    private Vector3 targetRotationAngle;
    private Vector3 currentRotationAngle;
    private Vector3 targetZoomPosition;
    private Vector3 currentZoomPosition;
    private bool horizontalEnabled = true;
    private Transform cam;

    public void Start()
    {
        targetRotationAngle = transform.eulerAngles;
        currentRotationAngle = transform.eulerAngles;

        cam = transform.GetChild(0).transform;
        targetZoomPosition = zoomPosition1;
        currentZoomPosition = cam.localPosition;
    }

    public void Update()
    {
        GetRotationInput();
        GetZoomInput();
        LerpRotation();
        LerpZoom();

        // TODO slerp a LookAt target instead of Camera Target, for a slightly more 3d feel even when not rotating. Without slerp it will make you sick.
        // Also combine this with FollowTarget script.. except FollowTarget script is used by Death Effect too.
        cam.LookAt(transform, Vector3.up);
    }

    private void GetRotationInput()
    {
        if (Input.GetAxis("Horizontal2") == 0f)
        {
            horizontalEnabled = true;
        }
        if ((Input.GetAxis("Horizontal2") < 0f && horizontalEnabled == true) || Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            targetRotationAngle = new Vector3(targetRotationAngle.x, targetRotationAngle.y + angleSnap, targetRotationAngle.z);
            horizontalEnabled = false;
        }
        if ((Input.GetAxis("Horizontal2") > 0f && horizontalEnabled == true) || Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            targetRotationAngle = new Vector3(targetRotationAngle.x, targetRotationAngle.y - angleSnap, targetRotationAngle.z);
            horizontalEnabled = false;
        }
    }

    private void GetZoomInput()
    {
        if (Input.GetAxis("Vertical2") == 0f && !canZoom)
        {
            canZoom = true;
        }
        else if (Input.GetAxis("Vertical2") < 0f && zoomPosition == 0 && canZoom)
        {
            zoomPosition = 1;
            targetZoomPosition = zoomPosition1;
            canZoom = false;
        }
        else if (Input.GetAxis("Vertical2") < 0f && zoomPosition == 1 && canZoom)
        {
            zoomPosition = 2;
            targetZoomPosition = zoomPosition2;
            canZoom = false;
        }
        else if (Input.GetAxis("Vertical2") > 0f && zoomPosition == 2 && canZoom)
        {
            zoomPosition = 1;
            targetZoomPosition = zoomPosition1;
            canZoom = false;
        }
        else if (Input.GetAxis("Vertical2") > 0f && zoomPosition == 1 && canZoom)
        {
            zoomPosition = 0;
            targetZoomPosition = zoomPosition0;
            canZoom = false;
        }
    }

    private void LerpRotation()
    {
        currentRotationAngle = new Vector3(
            Mathf.LerpAngle(currentRotationAngle.x, targetRotationAngle.x, Time.deltaTime * rotationSpeed),
            Mathf.LerpAngle(currentRotationAngle.y, targetRotationAngle.y, Time.deltaTime * rotationSpeed),
            Mathf.LerpAngle(currentRotationAngle.z, targetRotationAngle.z, Time.deltaTime * rotationSpeed));

        transform.eulerAngles = currentRotationAngle;
    }

    private void LerpZoom()
    {
        currentZoomPosition = new Vector3(
            Mathf.Lerp(currentZoomPosition.x, targetZoomPosition.x, Time.deltaTime * zoomSpeed),
            Mathf.Lerp(currentZoomPosition.y, targetZoomPosition.y, Time.deltaTime * zoomSpeed),
            Mathf.Lerp(currentZoomPosition.z, targetZoomPosition.z, Time.deltaTime * zoomSpeed));

        cam.localPosition = currentZoomPosition;
    }

    public float GetRotation()
    {
        return targetRotationAngle.y;
    }
}