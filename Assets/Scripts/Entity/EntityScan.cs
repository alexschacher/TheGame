using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityScan : MonoBehaviour
{
    private AIController aiController;

    private void Awake()
    {
        aiController = GetComponentInParent<AIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            if (other.gameObject.GetComponentInParent<Entity>() == null)
            {
                Debug.Log("Error: EntityScan found collider without entity: " + other.gameObject.transform.parent.name);
                return;
            }
            aiController.AddNearbyEntity(other.gameObject.GetComponentInParent<Entity>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            aiController.RemoveNearbyEntity(other.gameObject.GetComponentInParent<Entity>());
        }
    }
}
