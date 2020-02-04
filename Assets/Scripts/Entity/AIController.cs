using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Entity thisEntity;
    private Entity targetEntity;
    private EntityScan entityScan;

    private List<KeyValuePair<Entity, Entity.Disposition>>nearbyEntities =
        new List<KeyValuePair<Entity, Entity.Disposition>>();

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
        thisEntity = GetComponent<Entity>();
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
            if (nearbyEntities[i].Value == Entity.Disposition.Enemy &&
                nearbyEntities[i].Key.GetComponent<AIController>() != null)
            {
                if (nearbyEntities[i].Key.GetComponent<AIController>().GetTargetEntity() == thisEntity &&
                    nearbyEntities[i].Key.GetComponent<AIController>().GetCurrentBehaviorState() == BehaviorState.Attacking)
                {
                    SetDisposition(nearbyEntities[i].Key, Entity.Disposition.Hostile);
                }
            }
        }

        // If an ally has enemies or hostiles that I have marked neutral, mark as enemy
        for (int i = 0; i < nearbyEntities.Count; i++)
        {
            if (nearbyEntities[i].Value == Entity.Disposition.Allied &&
                nearbyEntities[i].Key.GetComponent<AIController>() != null)
            {
                List<Entity> listOfEnemiesAndHostiles = GetNearbyEnemiesAndHostiles(nearbyEntities[i].Key);

                for (int j = 0; j < listOfEnemiesAndHostiles.Count; j++)
                {
                    if (IsEntityInNearbyEntities(listOfEnemiesAndHostiles[j]))
                    {
                        if (GetDisposition(listOfEnemiesAndHostiles[j]) == Entity.Disposition.Neutral)
                        {
                            SetDisposition(listOfEnemiesAndHostiles[j], Entity.Disposition.Enemy);
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
                KeyValuePair<Entity, Entity.Disposition> a,
                KeyValuePair<Entity, Entity.Disposition> b)
            {
                float squaredRangeA = (a.Key.transform.position - thisEntity.transform.position).sqrMagnitude;
                float squaredRangeB = (b.Key.transform.position - thisEntity.transform.position).sqrMagnitude;
                return squaredRangeA.CompareTo(squaredRangeB);
            }
        );
    }

    private void ChooseTargetAndDetermineBehaviorState()
    {
        Entity closestHostile = null;
        float totalHostileThreat = 0f;

        // Look for hostiles
        foreach (KeyValuePair<Entity, Entity.Disposition> kvp in nearbyEntities)
        {
            if (kvp.Value == Entity.Disposition.Hostile)
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

        Entity closestEnemy = null;
        float totalEnemyThreat = 0f;

        // Look for enemies
        foreach (KeyValuePair<Entity, Entity.Disposition> kvp in nearbyEntities)
        {
            if (kvp.Value == Entity.Disposition.Enemy)
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
        if (!GameObject.Find("SceneControl").GetComponent<ConsoleCommand>().IsDebugVisible())
        {
            thisEntity.SetDebugText("");
            return;
        }

        string debugMsg1 = "";

        // Show Nearby Entities
        foreach (KeyValuePair<Entity, Entity.Disposition> kvp in nearbyEntities)
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

    private void ReactToTriggerDamagedByEntity(Entity e)
    {
        SetDisposition(e, Entity.Disposition.Hostile);
    }

    public Entity GetTargetEntity()
    {
        return targetEntity;
    }

    public BehaviorState GetCurrentBehaviorState()
    {
        return currentBehaviorState;
    }



    /************ List of KeyValuePair<Entity, Disposition> ******************/

    public List<Entity> GetNearbyEnemiesAndHostiles(Entity e)
    {
        List<Entity> listOfEnemiesAndHostiles = new List<Entity>();

        listOfEnemiesAndHostiles = listOfEnemiesAndHostiles.Union<Entity>(
            e.GetComponent<AIController>().
            GetNearbyEntitiesOfDisposition(Entity.Disposition.Enemy)).
            ToList<Entity>();

        listOfEnemiesAndHostiles = listOfEnemiesAndHostiles.Union<Entity>(
            e.GetComponent<AIController>().
            GetNearbyEntitiesOfDisposition(Entity.Disposition.Hostile)).
            ToList<Entity>();

        return listOfEnemiesAndHostiles;
    }

    public List<Entity> GetNearbyEntitiesOfDisposition(Entity.Disposition d)
    {
        List<Entity> list = new List<Entity>();
        foreach (KeyValuePair<Entity, Entity.Disposition> kvp in nearbyEntities)
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

    public bool IsEntityInNearbyEntities(Entity e)
    {
        return nearbyEntities.Any(kvp => kvp.Key == e);
    }

    public Entity.Disposition GetDisposition(Entity e)
    {
        return nearbyEntities.First(kvp => kvp.Key == e).Value;
    }

    private void SetDisposition(Entity e, Entity.Disposition d)
    {
        KeyValuePair<Entity, Entity.Disposition> newEntry = new KeyValuePair<Entity, Entity.Disposition>(e, d);
        RemoveNearbyEntity(e);
        nearbyEntities.Add(newEntry);
    }

    public void AddNearbyEntity(Entity e)
    {
        nearbyEntities.Add(new KeyValuePair<Entity, Entity.Disposition>(e, thisEntity.GetDisposition(e.GetAlignment())));
    }

    public void RemoveNearbyEntity(Entity e)
    {
        nearbyEntities.RemoveAll(kvp => kvp.Key.Equals(e));
        if (targetEntity == e)
        {
            targetEntity = null;
        }
    }
}