using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Run : AI_Behavior
{
    public override void Behave(Entity targetEntity)
    {
        thisEntity.CommandSetMovementVector(GetMovementVector(targetEntity));
    }

    private Vector3 GetMovementVector(Entity targetEntity)
    {
        return -(new Vector3(targetEntity.transform.position.x - thisEntity.transform.position.x,
                            0,
                            targetEntity.transform.position.z - thisEntity.transform.position.z).normalized);
    }
}
