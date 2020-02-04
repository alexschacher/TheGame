using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Behavior : MonoBehaviour
{
    protected Entity thisEntity;

    public abstract void Behave(Entity targetEntity);
    public virtual void SetEntity(Entity entity)
    {
        thisEntity = entity;
    }
}
