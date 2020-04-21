using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ChaseAndSwipe : AI_Behavior
{
    private float distanceToSwipe = 0.8f;

    public override void Behave(EntityOLD targetEntity)
    {
        thisEntity.CommandSetMovementVector(GetMovementVector(targetEntity));

        if (Vector3.Distance(targetEntity.transform.position, thisEntity.transform.position) < distanceToSwipe)
        {
            if (Vector3.Angle(thisEntity.GetMovementVector(), targetEntity.transform.position - thisEntity.transform.position) < 45)
            {
                thisEntity.CommandAttack();
            }
        }
    }

    private Vector3 GetMovementVector(EntityOLD targetEntity)
    {
        return (new Vector3(targetEntity.transform.position.x - thisEntity.transform.position.x,
                            0,
                            targetEntity.transform.position.z - thisEntity.transform.position.z).normalized);
    }
}
