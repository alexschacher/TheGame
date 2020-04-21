using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// NOTE!! REFACTOR, its becoming too large! Risk of becoming a God-Object!

public class EntityOLD : MonoBehaviour
{
    public enum Alignment
    {
        Player, Civilized, Monster
    }
    public enum Disposition
    {
        Allied, Neutral, Enemy, Hostile
    }

    [Header("")]
    [SerializeField] private Alignment alignment = Alignment.Monster;
    [SerializeField] private Disposition dispositionMonster = Disposition.Neutral;
    [SerializeField] private Disposition dispositionPlayer = Disposition.Neutral;
    [SerializeField] private Disposition dispositionCivilized = Disposition.Neutral;

    [Header("")]
    [SerializeField] private int threatLevel = 3;
    [SerializeField] private int threatBraveness = 3;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float walkSpeed = 250;

    private float jumpPower = 5;
    private float pushbackSpeed = 1200f;
    private float pushbackTime = 0.2f;
    private float stunTime = 0.2f;
    private float invulnerableTime = 0.2f;
    private float attackLength = 0.2f;

    private Text debugText;
    private Rigidbody rb;
    private Collider col;
    private EntityAnimator animator;
    public UnityAction<EntityOLD> eventDamagedByEntity;

    private bool isAttacking = false;
    private bool canAttack = true;
    private bool canWalk = true;
    private bool isInvulnerable = false;
    private Vector3 movementVector;
    private Vector3 controllerMovementVector;
    private float currentHealth;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = transform.GetComponentInChildren<Collider>();
        animator = GetComponentInChildren<EntityAnimator>();
        debugText = GetComponentInChildren<Text>();
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (canWalk)
        {
            SetVelocity(controllerMovementVector, walkSpeed);
            SetRotation();
            SetWalkAnimation();
        }

        // Weightier drop
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (1.5f) * Time.fixedDeltaTime;
        }
    }



    /************** Commands To Be Issued By Controller *****************/

    public void CommandSetMovementVector(Vector3 mv)
    {
        controllerMovementVector = mv;
    }

    public void CommandAttack()
    {
        if (canAttack && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    public void CommandJump()
    {
        if (IsGrounded() && (!isAttacking))
        {
            rb.velocity = Vector3.up * jumpPower;
        }
    }

    public void CommandTerminateJump()
    {
        // Weightier drop part 2
        if (rb.velocity.y > 0)
        {
            //rb.velocity += Vector3.up * Physics.gravity.y * (1f) * Time.fixedDeltaTime;
        }
    }

    public void CommandSufferPushback(Vector3 pushFrom)
    {
        StartCoroutine(SufferPushback(transform.position - pushFrom));
    }



    /************** Getters *****************/

    public Vector3 GetMovementVector()
    {
        return movementVector;
    }

    public Alignment GetAlignment()
    {
        return alignment;
    }

    public Disposition GetDisposition(Alignment towardsAlignment)
    {
        switch (towardsAlignment)
        {
            case Alignment.Civilized: return dispositionCivilized;
            case Alignment.Monster: return dispositionMonster;
            case Alignment.Player: return dispositionPlayer;
            default: return Disposition.Neutral;
        }
    }

    public bool IsVulnerable()
    {
        return !isInvulnerable;
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(
            col.bounds.center,
            new Vector3(col.bounds.center.x, col.bounds.min.y - 0.1f, col.bounds.center.z),
            0.3f,
            LayerMask.GetMask("Environment"));
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetThreatLevel()
    {
        return threatLevel;
    }

    public float GetThreatBraveness()
    {
        return threatBraveness;
    }



    /************** Setters *****************/

    private void SetVelocity(Vector3 vector, float speed)
    {
        movementVector = vector.normalized;
        rb.velocity = new Vector3(
            movementVector.x * speed * Time.deltaTime,
            rb.velocity.y,
            movementVector.z * speed * Time.deltaTime);
    }

    private void SetRotation()
    {
        if (System.Math.Abs(movementVector.x) > 0 || System.Math.Abs(movementVector.z) > 0)
        {
            transform.forward = movementVector.normalized;
        }
    }

    private void SetWalkAnimation()
    {
        if (movementVector.x == 0 && movementVector.z == 0)
        {
            animator.StartAnimation(EntityAnimator.Anim.Stand);
        }
        else
        {
            animator.StartAnimation(EntityAnimator.Anim.Walk);
        }
    }

    public void SetDebugText(string text)
    {
        if (debugText != null)
        {
            debugText.text = text;
        }
    }



    /************** Attacking *****************/

    private IEnumerator Attack()
    {
        isAttacking = true;
        canAttack = false;
        canWalk = false;
        animator.StartAnimation(EntityAnimator.Anim.Attack);
        SetVelocity(Vector3.zero, 0f);

        GameObject obj = Instantiate(Resources.Load("Pfb_Effects/Pfb_Effect_Swipe") as GameObject, transform);
        obj.GetComponent<Swipe>().SetOriginEntity(this);
        if (GetComponent<AIController>() != null)
        {
            obj.GetComponent<Swipe>().SetTargets(GetComponent<AIController>().GetNearbyEnemiesAndHostiles(this));
        }
        else
        {
            obj.GetComponent<Swipe>().SetDamageAny(true);
        }

        yield return new WaitForSeconds(attackLength);

        isAttacking = false;
        canAttack = true;
        canWalk = true;
        animator.StartAnimation(EntityAnimator.Anim.Stand);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HurtEntityOnTouch(collision);
    }

    private void HurtEntityOnTouch(Collision collision)
    {
        EntityOLD touchedEntity = collision.collider.GetComponentInParent<EntityOLD>();
        if (touchedEntity != null)
        {
            if (GetComponent<AIController>() != null)
            {
                if (GetComponent<AIController>().GetNearbyEnemiesAndHostiles(this).Contains(touchedEntity))
                {
                    if (touchedEntity.GetComponent<AIController>() != null)
                    {
                        touchedEntity.TriggerEventDamagedByEntity(this);
                    }
                    touchedEntity.ModifyHealth(-10f);
                    touchedEntity.CommandSufferPushback(transform.position);
                }
            }
        }
    }



    /************** Taking Damage *****************/

    public IEnumerator SufferPushback(Vector3 pushbackVector)
    {
        canWalk = false;
        canAttack = false;
        isInvulnerable = true;
        SetVelocity(pushbackVector, pushbackSpeed);
        animator.StartAnimation(EntityAnimator.Anim.Hurt);

        if (currentHealth <= 0)
        {
            GameObject obj = Instantiate(Resources.Load("Pfb_Effects/Pfb_Effect_Death") as GameObject, transform.position, Quaternion.identity);
            obj.GetComponent<FollowTarget>().SetTarget(this.transform);
        }

        yield return new WaitForSeconds(pushbackTime);

        SetVelocity(pushbackVector, 0);

        if (currentHealth <= 0)
        {
            Die();
        }

        yield return new WaitForSeconds(stunTime);

        canWalk = true;
        canAttack = true;

        yield return new WaitForSeconds(invulnerableTime);

        isInvulnerable = false;
    }

    public void ModifyHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void TriggerEventDamagedByEntity(EntityOLD e)
    {
        eventDamagedByEntity.Invoke(e);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}