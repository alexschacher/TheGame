using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// NOTE!! Consider refactoring, its becoming too large! Risk of God-Object!

public class AIController : MonoBehaviour
{
    private EntityOLD thisEntity;
    private EntityOLD targetEntity;
    private EntityScan entityScan;

    private List<KeyValuePair<EntityOLD, EntityOLD.Disposition>>nearbyEntities =
        new List<KeyValuePair<EntityOLD, EntityOLD.Disposition>>();

    public enum BehaviorState
    {
        Idling, Attacking, Running
    }
    private BehaviorState currentBehaviorState;

    private Vector2 randomBehaviorChangeInterval = new Vector2(0.1f, 0.9f);
    private float chanceToAttackInsteadOfRun = 0.03f;

    

    [SerializeField] private AI_Behavior AI_idle;
    [SerializeField] private AI_Behavior AI_run;
    [SerializeField] private AI_Behavior AI_attack;

    void Awake()
    {
        thisEntity = GetComponent<EntityOLD>();
        thisEntity.eventDamagedByEntity += ReactToTriggerDamagedByEntity;
        entityScan = GetComponentInChildren<EntityScan>();

        AI_idle.SetEntity(thisEntity);
        AI_run.SetEntity(thisEntity);
        AI_attack.SetEntity(thisEntity);

        currentBehaviorState = BehaviorState.Idling;
    }



    /************** Update Loop *****************/

    void Update()
    {
        RemoveNullEntities();
        CheckDispositions();
        SortEntitiesByDistance();
        ChooseTargetAndDetermineBehaviorState();
        ActOnBehavior();
        UpdateDebugText();
    }

    private void CheckDispositions()
    {
        // If enemy is coming to attack me, mark them hostile
        for (int i = 0; i < nearbyEntities.Count; i++)
        {
            if (nearbyEntities[i].Value == EntityOLD.Disposition.Enemy &&
                nearbyEntities[i].Key.GetComponent<AIController>() != null)
            {
                if (nearbyEntities[i].Key.GetComponent<AIController>().GetTargetEntity() == thisEntity &&
                    nearbyEntities[i].Key.GetComponent<AIController>().GetCurrentBehaviorState() == BehaviorState.Attacking)
                {
                    SetDisposition(nearbyEntities[i].Key, EntityOLD.Disposition.Hostile);
                }
            }
        }

        // If an ally has enemies or hostiles that I have marked neutral, mark as enemy
        for (int i = 0; i < nearbyEntities.Count; i++)
        {
            if (nearbyEntities[i].Value == EntityOLD.Disposition.Allied &&
                nearbyEntities[i].Key.GetComponent<AIController>() != null)
            {
                List<EntityOLD> listOfEnemiesAndHostiles = GetNearbyEnemiesAndHostiles(nearbyEntities[i].Key);

                for (int j = 0; j < listOfEnemiesAndHostiles.Count; j++)
                {
                    if (IsEntityInNearbyEntities(listOfEnemiesAndHostiles[j]))
                    {
                        if (GetDisposition(listOfEnemiesAndHostiles[j]) == EntityOLD.Disposition.Neutral)
                        {
                            SetDisposition(listOfEnemiesAndHostiles[j], EntityOLD.Disposition.Enemy);
                        }
                    }
                }
            }
        }
    }

    private void SortEntitiesByDistance()
    {
        nearbyEntities.Sort(
            delegate (
                KeyValuePair<EntityOLD, EntityOLD.Disposition> a,
                KeyValuePair<EntityOLD, EntityOLD.Disposition> b)
            {
                float squaredRangeA = (a.Key.transform.position - thisEntity.transform.position).sqrMagnitude;
                float squaredRangeB = (b.Key.transform.position - thisEntity.transform.position).sqrMagnitude;
                return squaredRangeA.CompareTo(squaredRangeB);
            }
        );
    }

    private void ChooseTargetAndDetermineBehaviorState()
    {
        EntityOLD closestHostile = null;
        float totalHostileThreat = 0f;

        // Look for hostiles
        foreach (KeyValuePair<EntityOLD, EntityOLD.Disposition> kvp in nearbyEntities)
        {
            if (kvp.Value == EntityOLD.Disposition.Hostile)
            {
                totalHostileThreat += kvp.Key.GetThreatLevel();

                if (closestHostile == null)
                {
                    closestHostile = kvp.Key;
                }
            }
        }

        // Determine based on threat whether to run or attack,
        // With a 1% chance to attack when they would normally run
        if (totalHostileThreat > thisEntity.GetThreatBraveness() &&
            Random.Range(0f, 1f) > chanceToAttackInsteadOfRun)
        {
            currentBehaviorState = BehaviorState.Running;
        }
        else
        {
            currentBehaviorState = BehaviorState.Attacking;
        }

        // If hostile found, target them
        if (closestHostile != null)
        {
            targetEntity =  closestHostile;
            return;
        }
        // No hostiles found...

        EntityOLD closestEnemy = null;
        float totalEnemyThreat = 0f;

        // Look for enemies
        foreach (KeyValuePair<EntityOLD, EntityOLD.Disposition> kvp in nearbyEntities)
        {
            if (kvp.Value == EntityOLD.Disposition.Enemy)
            {
                totalEnemyThreat += kvp.Key.GetThreatLevel();

                if (closestEnemy == null)
                {
                    closestEnemy = kvp.Key;
                }
            }
        }

        // Determine based on threat whether to run or attack
        if (totalEnemyThreat > thisEntity.GetThreatBraveness() &&
            Random.Range(0f, 1f) > chanceToAttackInsteadOfRun)
        {
            currentBehaviorState = BehaviorState.Running;
        }
        else
        {
            currentBehaviorState = BehaviorState.Attacking;
        }

        // If hostile found, target them
        if (closestEnemy != null)
        {
            targetEntity =  closestEnemy;
            return;
        }

        // No hostiles or enemies found, don't target anyone
        currentBehaviorState = BehaviorState.Idling;
        targetEntity = null;
    }

    private void ActOnBehavior()
    {
        // If i dont have a target
        if (targetEntity == null)
        {
            AI_idle.Behave(thisEntity);
        }

        // If i am targeting another entity
        if (targetEntity != null)
        {
            if (currentBehaviorState == BehaviorState.Running)
            {
                AI_run.Behave(targetEntity);
            }
            else if (currentBehaviorState == BehaviorState.Attacking)
            {
                AI_attack.Behave(targetEntity);
            }
        }
    }

    private void UpdateDebugText()
    {
        if (!GameObject.Find("Program").GetComponent<ConsoleCommand>().IsDebugVisible())
        {
            thisEntity.SetDebugText("");
            return;
        }

        string debugMsg1 = "";

        // Show Nearby Entities
        foreach (KeyValuePair<EntityOLD, EntityOLD.Disposition> kvp in nearbyEntities)
        {
            debugMsg1 += (GetDisposition(kvp.Key) + " to " + kvp.Key.transform.name + "\n");
        }

        // Show Behavior
        debugMsg1 += currentBehaviorState.ToString();

        // Show Target
        if (targetEntity != null)
        {
            string targetName = targetEntity.transform.name;
            targetName = targetName.Replace("Pfb_Entity_", "");
            targetName = targetName.Replace("AI_", "");
            debugMsg1 += " " + GetDisposition(targetEntity) + " " + targetName;
        }

        // Show Health
        // debugMsg1 += "\n" + thisEntity.GetCurrentHealth() + "/" + thisEntity.GetMaxHealth();

        thisEntity.SetDebugText(debugMsg1);
    }



    /****************** Misc *************************/

    private void ReactToTriggerDamagedByEntity(EntityOLD e)
    {
        SetDisposition(e, EntityOLD.Disposition.Hostile);
    }

    public EntityOLD GetTargetEntity()
    {
        return targetEntity;
    }

    public BehaviorState GetCurrentBehaviorState()
    {
        return currentBehaviorState;
    }



    /************ List of KeyValuePair<Entity, Disposition> ******************/

    public List<EntityOLD> GetNearbyEnemiesAndHostiles(EntityOLD e)
    {
        List<EntityOLD> listOfEnemiesAndHostiles = new List<EntityOLD>();

        listOfEnemiesAndHostiles = listOfEnemiesAndHostiles.Union<EntityOLD>(
            e.GetComponent<AIController>().
            GetNearbyEntitiesOfDisposition(EntityOLD.Disposition.Enemy)).
            ToList<EntityOLD>();

        listOfEnemiesAndHostiles = listOfEnemiesAndHostiles.Union<EntityOLD>(
            e.GetComponent<AIController>().
            GetNearbyEntitiesOfDisposition(EntityOLD.Disposition.Hostile)).
            ToList<EntityOLD>();

        return listOfEnemiesAndHostiles;
    }

    public List<EntityOLD> GetNearbyEntitiesOfDisposition(EntityOLD.Disposition d)
    {
        List<EntityOLD> list = new List<EntityOLD>();
        foreach (KeyValuePair<EntityOLD, EntityOLD.Disposition> kvp in nearbyEntities)
        {
            if (kvp.Value == d)
            {
                list.Add(kvp.Key);
            }
        }
        return list;
    }

    private void RemoveNullEntities()
    {
        nearbyEntities.RemoveAll(kvp => kvp.Key.Equals(null));
    }

    public bool IsEntityInNearbyEntities(EntityOLD e)
    {
        return nearbyEntities.Any(kvp => kvp.Key == e);
    }

    public EntityOLD.Disposition GetDisposition(EntityOLD e)
    {
        return nearbyEntities.First(kvp => kvp.Key == e).Value;
    }

    private void SetDisposition(EntityOLD e, EntityOLD.Disposition d)
    {
        KeyValuePair<EntityOLD, EntityOLD.Disposition> newEntry = new KeyValuePair<EntityOLD, EntityOLD.Disposition>(e, d);
        RemoveNearbyEntity(e);
        nearbyEntities.Add(newEntry);
    }

    public void AddNearbyEntity(EntityOLD e)
    {
        nearbyEntities.Add(new KeyValuePair<EntityOLD, EntityOLD.Disposition>(e, thisEntity.GetDisposition(e.GetAlignment())));
    }

    public void RemoveNearbyEntity(EntityOLD e)
    {
        nearbyEntities.RemoveAll(kvp => kvp.Key.Equals(e));
        if (targetEntity == e)
        {
            targetEntity = null;
        }
    }
}