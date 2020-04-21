using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Collider collider;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = transform.GetComponentInChildren<Collider>();
    }



    /*  ENTITY CLASS PLANNING AND ORGANIZATION
     
        To issue a command to EntityComponents:
        if (otherEntity.TryGetComponent (out EntityAnimator animator))
        {
             animator.Animate(EntityAnimator.Anim.Walk);
        }




        [ ] Entity.cs				EntityContainer Data, Rigidbody, Collider, Anything that ALL Entities share

        [ ] EntityTileObject.cs     These objects are saved and loaded in the world grid. They are usually(?) static. (What about moving blocks? Spawnable Object?)
        [ ] EntitySpawnedObject.cs  An object spawned by a TileObject, holds a reference to its spawner.
        [ ] EntitySpawner.cs        A TileObject that spawns a SpawnedObject. Holds a list of its SpawnedObjects, so it can clear its spawned, or know whether or not to spawn a new one, etc.
        [ ] EntityTerrain.cs        Tile Object that checks its Tile neighbors for similar terrain, in order to determine model. Has "health" that can be chipped away etc for curved/slanted blocks etc.
        [ ] EntityLiquid.cs         Water/Lava etc: Checks for neighbor liquids, can flow into waterfalls, etc. Potentially a dynamic flowing and pooling system.

        [ ] EntityController.cs		Player, AI, or Inanimate if not included
        [ ] EntitySentience.cs		Dynamic Relation to Others: Alignment, Disposition, Threat, Braveness (Tied to EntityController? Any reason they are not exclusive to each other?)
            Cases:
            An object that AI are afraid of or will attack, even if that object is inanimate.
            Braveness only applies to AI; not Player or Inanimate. Disposition can be applied to player or AI, not Inanimate. Alignment and Threat can be applied to any.
                Sentience is mainly used for AI, to determine how to act.
                Sentience for player uses disposition simply to prevent friendly fire.
                Otherwise, Sentience for players and inanimate is only used for AI to interact with in their decision making process.

        [ ] EntityAnimator.cs		Animations
        [?] EntityLitByShader.cs       Is lit by the lighting shader.

        [ ] EntityDestructable.cs		Mortality: Health, Taking Damage, Dieing, Death can spawn an Effect Entity
        [ ] EntityReactToForces.cs		Pushback from Attacks, Exlosions, etc.

        [~] EntityTalk.cs			Ability to hold or generate messages and display them in a chat bubble. Players, AI, Signs, etc.
        [ ] EntityAttack.cs			Attacking: Strength, Length, isAttacking, canAttack, Attack(), HurtOnTouch()
        [ ] EntityWalk.cs			Walking: Walkspeed, canWalk, movementVector, controllerMovementVector
        [~] EntityJump.cs			Jumping: Jump Power, Jump()
        [?] EntitySwim				Float in water, sink, or swim
        [?] EntityFly				Float in air or fly

        [~] EntityHoldable.cs		Can be picked up by someone with CarryOther
        [~] EntityCanHold.cs		Can hold Carryables. Can pick up, set down, and throw Carryables	(Inanimate can be a barrel that holds resource Entities)
        [?] EntityPickupable.cs		Pickupable item such as coins or hearts
        [?] EntityCanPickup.cs		Pickup items that are Pickupable.
        [?] EntityPushable.cs		Objects that can be pushed or pulled, such as a crate.
        [?] EntityCanPush.cs		Can push pushable objects.

        [?] EntityToolUser.cs		Can use tools: Axe, pickaxe, hoe, fishingrod, etc.
        [?] EntityWeaponUser.cs		Can equip and use weapons (Tied to EntityAttack?)
        [?] EntityArmorUser.cs		Can equip armor (Tied to EntityMortal?)
        [?] EntityInventory.cs		Has an internal inventory of items (May not be a mechanic in this game?)

        [?] EntityGeneratePower.cs	Generates power somehow.
        [?] EntityUsePower.cs		Accepts power and creates an effect, ie: message, change block.
        [?] EntityTransferPower.cs	Transfers power from A to B.


            Entity Possibilities:
			Characters (Player, NPC, Enemies)
			Dynamic Carryable Objects (Pots, Resources, Crops, Bombs)
			Attacks (Swipe, Explosion, Arrow)
			Dropped pickup items (coins, hearts)
			Pushable objects such as crates
			Power producers, conducters, and users, such as buttons, wires, and gates.

			Entity Component Interface:
			- Commands can be issued to Entity.cs, which will pass it on to a relevant component if it exists
			- Command examples: entity.Damage(amount); entity.Walk(direction, speed); entity.Destroy(); entity.Animate(Anim);

            Entity Factory:
            Attach components per Entity, not all Entities need to Jump, or Attack, or even Move
            Have default constructor method for basic entity types, such as humanoid who all share the same components
            Entities should be able to function with or without any component, commands that are irrelevant are just ignored
    */

}


