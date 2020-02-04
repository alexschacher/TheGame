using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Wander : AI_Behavior
{
    [SerializeField] private float minMoveTime = 0.2f;
    [SerializeField] private float maxMoveTime = 1.5f;
    [SerializeField] private float minStopTime = 0.2f;
    [SerializeField] private float maxStopTime = 3f;

    private bool isMoving = false;
    private float timeToSwitch;

    public override void Behave(Entity targetEntity)
    {
        if (Time.time > timeToSwitch)
        {
            if (isMoving)
            {
                thisEntity.CommandSetMovementVector(StopMoving());
                timeToSwitch = Time.time + Random.Range(minStopTime, maxStopTime);
                isMoving = false;
            }
            else
            {
                thisEntity.CommandSetMovementVector(SetRandomMovementVector());
                timeToSwitch = Time.time + Random.Range(minMoveTime, maxMoveTime);
                isMoving = true;
            }
        }
    }

    private Vector3 StopMoving()
    {
        return Vector3.zero;
    }
    private Vector3 SetRandomMovementVector()
    {
        return new Vector3(Random.Range(-1f, 1f),
                           0,
                           Random.Range(-1f, 1f));
    }
}