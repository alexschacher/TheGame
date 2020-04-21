using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI_Behavior : MonoBehaviour
{
    protected EntityOLD thisEntity;

    public abstract void Behave(EntityOLD targetEntity);
    public virtual void SetEntity(EntityOLD entity)
    {
        thisEntity = entity;
    }
}
