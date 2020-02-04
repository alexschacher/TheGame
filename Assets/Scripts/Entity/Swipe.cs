 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    [SerializeField] private float destructDelay = 0.1f;
    [SerializeField] private float damage = 40f;

    private List<Entity> targets;
    private bool damageAny = false;
    private Entity originEntity;

    void Awake()
    {
        Destroy(gameObject, destructDelay);
        targets = new List<Entity>();
    }

    public void SetOriginEntity(Entity entity)
    {
        originEntity = entity;
    }

    public void SetTargets(List<Entity> list)
    {
        targets = list;
    }

    public void SetDamageAny(bool b)
    {
        damageAny = b;
    }

    void OnTriggerEnter(Collider collider)
    {
        Entity entity = collider.gameObject.GetComponentInParent<Entity>();

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