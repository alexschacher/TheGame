 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    [SerializeField] private float destructDelay = 0.1f;
    [SerializeField] private float damage = 40f;

    private List<EntityOLD> targets;
    private bool damageAny = false;
    private EntityOLD originEntity;

    void Awake()
    {
        Destroy(gameObject, destructDelay);
        targets = new List<EntityOLD>();
    }

    public void SetOriginEntity(EntityOLD entity)
    {
        originEntity = entity;
    }

    public void SetTargets(List<EntityOLD> list)
    {
        targets = list;
    }

    public void SetDamageAny(bool b)
    {
        damageAny = b;
    }

    void OnTriggerEnter(Collider collider)
    {
        EntityOLD entity = collider.gameObject.GetComponentInParent<EntityOLD>();

        if (entity != null)
        {
            if (entity.IsVulnerable() && (targets.Contains(entity) || damageAny == true))
            {
                if (entity.GetComponent<AIController>() != null)
                {
                    entity.TriggerEventDamagedByEntity(originEntity);
                }
                entity.ModifyHealth(-damage);
                entity.CommandSufferPushback(originEntity.transform.position);
            }
        }

        else
        {
            Destructable destructable = collider.gameObject.GetComponent<Destructable>();

            if (destructable != null)
            {
                destructable.Damage(damage);
            }
        }
    }
}