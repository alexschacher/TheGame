using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GodToolPlaceObjects allows placing and deleting objects from a level editor perspective.

public class GodToolPlaceObjects : MonoBehaviour
{
    private Ray pointRay;
    private Vector3 gridPosition;
    private float blockHeight = 0.75f;

    private bool viewDragging = false;
    Vector3 viewDragCamStartPos;
    Vector3 viewDragMouseStartPos;

    private WorldController worldController;
    private int placeHeight = 0;
    [SerializeField] private Tile.ID idToPlace = Tile.ID.BlockGrass;
    // DEPRECATED: [SerializeField] private GameObject objectToPlace;


    private void Awake()
    {
        worldController = GameObject.Find("WorldControl").GetComponent<WorldController>();
    }

    private void Update()
    {
        // Update Point Ray
        pointRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        UpdateGridPosition();
        
        CheckLeftClickToPlace();
        CheckRightClickToDelete();
        CheckSelectObject();
        CheckHeightChange();
    }

    private void UpdateGridPosition()
    {
        Plane plane = new Plane(Vector3.up, -placeHeight * blockHeight);
        float distanceToPlane;
        plane.Raycast(pointRay, out distanceToPlane);
        
        gridPosition = new Vector3(
            Mathf.RoundToInt(pointRay.GetPoint(distanceToPlane).x),
            placeHeight * blockHeight,
            Mathf.RoundToInt(pointRay.GetPoint(distanceToPlane).z));

        this.gameObject.transform.GetChild(0).transform.position = gridPosition;
    }

    private void CheckLeftClickToPlace()
    {
        // Left click to place
        if (Input.GetMouseButton(0))
        {
            worldController.ModifyWorldData((int)gridPosition.x, (int)gridPosition.z, placeHeight, idToPlace);


            // DEPRECATED: Places object from prefab without involving World Controller
            //if (objectToPlace != null)
            //{
                //Instantiate(objectToPlace, gridPosition, Quaternion.identity);
            //}
        }
    }

    private void CheckRightClickToDelete()
    {
        // Right click to delete
        if (Input.GetMouseButton(1))
        {
            worldController.ModifyWorldData((int)gridPosition.x, (int)gridPosition.z, placeHeight, Tile.ID.Empty);


            // DEPRECATED: Deletes objects without involving World Controller
            //RaycastHit hit;

            // Only find entities and environment colliders
            //if (Physics.Raycast(pointRay, out hit, Mathf.Infinity, LayerMask.GetMask("Entity", "Environment")))
            {
                // Destroy parent object
                //if (hit.transform.parent != null)
                {
                    //Destroy(hit.transform.parent.gameObject);
                }

                // Else destroy actual object
                //else
                {
                    //Destroy(hit.transform.gameObject);
                }

            }
        }
    }

    private void CheckSelectObject()
    {
        // Middle click to select Tile.ID for placement
        if (Input.GetMouseButton(2))
        {
            Tile.ID newId = worldController.GetWorldDataContainer().GetData((int)gridPosition.x, (int)gridPosition.z, placeHeight);

            if (newId != Tile.ID.Empty && newId != Tile.ID.EmptyButBlocked)
            {
                idToPlace = newId;
            }
        }
    }

    private void CheckHeightChange()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (placeHeight < worldController.GetWorldDataContainer().GetHeight() - 1)
            {
                placeHeight += 1;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (placeHeight > 0)
            {
                placeHeight -= 1;
            }
        }
    }
}