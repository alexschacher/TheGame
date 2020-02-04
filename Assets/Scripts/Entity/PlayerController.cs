using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private string playerNumber = "1";

    private Entity entity;

    void Start()
    {
        entity = GetComponent<Entity>();
    }

    void Update()
    {
        // Debug Text
        if (GameObject.Find("SceneControl").GetComponent<ConsoleCommand>().IsDebugVisible())
        {
            entity.SetDebugText("P" + playerNumber);
        }
        else
        {
            entity.SetDebugText("");
        }

        // Debug Invincible
        if (GameObject.Find("SceneControl").GetComponent<ConsoleCommand>().IsInvincible())
        {
            entity.ModifyHealth(1000);
        }

        // Jump Control
        if (Input.GetButtonDown("Jump_P" + playerNumber))
        {
            entity.CommandJump();
        }
        else
        {
            //entity.CommandTerminateJump();
        }

        // Attack Control
        if (Input.GetButtonDown("Attack_P" + playerNumber))
        {
            entity.CommandAttack();
        }

        // Movement Control
        entity.CommandSetMovementVector(GetMovementVector());
    }

    private Vector3 GetMovementVector()
    {
        // Camera direction
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        // Input direction
        Vector3 movementVector = (camForward * Input.GetAxisRaw("Walk_Vertical_P" + playerNumber))
                               + (camRight * Input.GetAxisRaw("Walk_Horizontal_P" + playerNumber));
        movementVector = movementVector.normalized;

        return movementVector;
    }
}
