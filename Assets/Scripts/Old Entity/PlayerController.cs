using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private string playerNumber = "1";

    private EntityOLD entity;

    void Start()
    {
        entity = GetComponent<EntityOLD>();
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
        entity.CommandSetMovementVector(
            MovementVectorCameraConverter.convertMovementVector(
                Input.GetAxisRaw("Walk_Vertical_P" + playerNumber),
                Input.GetAxisRaw("Walk_Horizontal_P" + playerNumber)));
    }
}
