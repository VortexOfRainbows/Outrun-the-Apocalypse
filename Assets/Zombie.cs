using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Zombie : EntityWithCharDrawing
{
    /// <summary>
    /// Determines the speed at which projectiles owned by the zombie should travel at compared to player speeds
    /// </summary>
    public float projectileSpeedMultiplier { get; private set; } //Is public on purpose, as it is accessed elsewhere.
    [SerializeField] private float movespeedMultiplier = 1.4f;
    [SerializeField] private float directMovespeedMultiplier = 1.4f;
    [SerializeField] private float InertiaPercent = 0.2f;
    [SerializeField] private CharacterAnimator CharacterAnimator;
    [SerializeField] private float baseDropChance = 0.2f;
    private GameObject player;
    public override int LayerDefaultPosition => -20;
    public override float ArmDegreesOffset => 90;
    public Vector2 DrunkVelocity; //A random velocity offset that is constantly added to the zombie in order to tilt it
    public bool FollowDirect; //Whether the zombies follows directly, or follows with a square pattern
    public override void SetStats()
    {
        projectileSpeedMultiplier = 0.5f;
        FollowDirect = Random.Range(0.0f, 1.0f) < 0.5;
        DrunkVelocity = new Vector2(0.3f, 0.0f).RotatedBy(Random.Range(0, Mathf.PI * 2));
        MaxLife = 18;
        Life = 18;
        ContactDamage = 15;
        Friendly = false;
    }
    [SerializeField] private float chanceForCorn = 0.1f; //Items appearing on Zombies becomes more common as time progresses
    [SerializeField] private float chanceForFarmerGun = 0.035f;
    [SerializeField] private float chanceForSpectre = 0.015f;
    [SerializeField] private float chanceForMortar = 0.01f;
    [SerializeField] private float chanceForOdin = 0.0075f;
    [SerializeField] private float chanceForPotatoGun = 0.0025f;
    public void AssignItem(ref ItemData item)
    {
        float difficulty = EnemyScalingFactor - 1; //starts at 0, Linear scaling as time progresses. Until it reaches 1, at which sqrt scaling begins
        if(chanceForCorn * (1 + difficulty) > Random.Range(0, 1f))
        {
            item = new Corn();
        }
        else if (chanceForFarmerGun * difficulty > Random.Range(0, 1f))
        {
            item = new FarmerGun();
        }
        else if (chanceForSpectre * difficulty > Random.Range(0, 1f))
        {
            item = new Spectre();
        }
        else if (chanceForMortar * difficulty > Random.Range(0, 1f))
        {
            item = new MortarCannon();
        }
        else if (chanceForOdin * difficulty > Random.Range(0, 1f))
        {
            item = new Odin();
        }
        else if (chanceForPotatoGun * difficulty > Random.Range(0, 1f))
        {
            item = new PotatoGun();
        }
        else
        {
            item = new NoItem();
        }
    }
    public override void OnFixedUpdate()
    {
        if (player == null) 
            player = Player.MainPlayer.gameObject;
        if (LeftHeldItem == null)
            AssignItem(ref LeftHeldItem);
        if (RightHeldItem == null)
            AssignItem(ref RightHeldItem);
        Velocity *= InertiaPercent; //using velocity to update position because it helps instruct the animator what to do in order to animate the zombie
        if(FollowDirect)
        {
            Vector2 toPlayer = player.transform.position - transform.position;
            Velocity += toPlayer.normalized * directMovespeedMultiplier * movespeedMultiplier * (1 - InertiaPercent);
        }
        else
        {
            if (transform.position.x < player.transform.position.x - 5) //These 5 are simply in place to prevent the zombie from jittering in its movements. They don't really need to be variables because this number is mostly inconsequential otherwise.
            {
                Velocity.x += movespeedMultiplier * (1 - InertiaPercent); //Increase velocity by 1 to the right, since the player is right of the zombie
            }
            if (transform.position.x > player.transform.position.x + 5)
            {
                Velocity.x -= movespeedMultiplier * (1 - InertiaPercent);
            }
            if (transform.position.y < player.transform.position.y - 5)
            {
                Velocity.y += movespeedMultiplier * (1 - InertiaPercent);
            }
            if (transform.position.y > player.transform.position.y + 5)
            {
                Velocity.y -= movespeedMultiplier * (1 - InertiaPercent);
            }
        }
        Velocity += DrunkVelocity;
        Velocity *= EnemyScalingFactor;
        if (Velocity.x > 0)
            Direction = 1;
        else
            Direction = -1;
        rb.velocity = Velocity;

        LookTarget = player.transform.position;
        CharacterAnimator.PerformUpdate();
        LastDirection = Direction;

        if (DecideToUseItem(LeftHeldItem))
        {
            if (LeftHeldItem.UseItem(this, CharacterAnimator.LeftItem))
            {
                LeftHeldItem = new NoItem();
            }
        }
        if (DecideToUseItem(RightHeldItem))
        {
            if (RightHeldItem.UseItem(this, CharacterAnimator.RightItem))
            {
                RightHeldItem = new NoItem();
            }
        }
    }
    public bool DecideToUseItem(ItemData item)
    {
        if (item != null && item is not NoItem)
        {
            if(item is Corn)
            {
                if(((MaxLife - Life) / item.Damage * (1 - Life / MaxLife)) > Random.Range(0, 1f)) //only use corn if it will heal. greater chance the lower the enemy health is
                {
                    float randomChanceToUseEachFrame = 0.01f * EnemyScalingFactor;
                    if (randomChanceToUseEachFrame > Random.Range(0, 1f))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if(item.UseCooldown > 0)
                {
                    float chanceToUse = 0.5f / item.UseCooldown;
                    if (chanceToUse > Random.Range(0, 1f))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public void DropItems(ref ItemData item)
    {
        if(item is not NoItem)
        {
            Vector2 random = new Vector2(1, 0).RotatedBy(Random.Range(0, Mathf.PI * 2));
            float dropChance = baseDropChance * Mathf.Sqrt(EnemyScalingFactor); //drop rates should get more common as you get into the game.
            if (item is Corn || dropChance > Random.Range(0, 1f)) //guaranteed to drop corn. Otherwise, drop rate is low
            {
                ItemData.NewItem(item, transform.position, random);
            }
        }
    }
    public void GenerateGore()
    {

    }
    public override void OnDeath()
    {
        AudioManager.instance.Play("ZombieDeath");
        GameObject coin = Instantiate(PrefabManager.GetPrefab("coin"), transform.position, new Quaternion());
        coin.GetComponent<Coin>().DespawnCounter = 0;
        DropItems(ref LeftHeldItem);
        DropItems(ref RightHeldItem);
        LeftHeldItem = new NoItem();
        RightHeldItem = new NoItem();
    }
}
