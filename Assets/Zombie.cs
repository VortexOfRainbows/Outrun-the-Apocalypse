using UnityEngine;

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
        MaxLife = 17;
        Life = MaxLife;
        ContactDamage = 14;
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
    [SerializeField] private float SpeedMultiplierOutOfRange = 2f;
    [SerializeField] private float ActivateOutOfRangeDistance = 200f;
    public override void OnFixedUpdate()
    {
        if (player == null) 
            player = Player.MainPlayer.gameObject;
        if (LeftHeldItem == null)
            AssignItem(ref LeftHeldItem);
        if (RightHeldItem == null)
            AssignItem(ref RightHeldItem);
        Velocity *= InertiaPercent; //using velocity to update position because it helps instruct the animator what to do in order to animate the zombie
        Vector2 toPlayer = player.transform.position - transform.position;
        float movespeed = movespeedMultiplier * Mathf.Sqrt(EnemyScalingFactor);
        if(toPlayer.magnitude > ActivateOutOfRangeDistance)
        {
            movespeed *= SpeedMultiplierOutOfRange;
        }
        if (FollowDirect)
        {
            Velocity += toPlayer.normalized * directMovespeedMultiplier * movespeed * (1 - InertiaPercent);
        }
        else
        {
            if (transform.position.x < player.transform.position.x - 5) //These 5 are simply in place to prevent the zombie from jittering in its movements. They don't really need to be variables because this number is mostly inconsequential otherwise.
            {
                Velocity.x += movespeed * (1 - InertiaPercent); //Increase velocity by 1 to the right, since the player is right of the zombie
            }
            if (transform.position.x > player.transform.position.x + 5)
            {
                Velocity.x -= movespeed * (1 - InertiaPercent);
            }
            if (transform.position.y < player.transform.position.y - 5)
            {
                Velocity.y += movespeed * (1 - InertiaPercent);
            }
            if (transform.position.y > player.transform.position.y + 5)
            {
                Velocity.y -= movespeed * (1 - InertiaPercent);
            }
        }
        Velocity += DrunkVelocity;
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
    [SerializeField] private float EatCornBaseChance = 0.0125f;
    public bool DecideToUseItem(ItemData item)
    {
        if (item != null && item is not NoItem)
        {
            if(item is Corn)
            {
                if(((MaxLife - Life) / item.Damage * (1 - Life / MaxLife)) > Random.Range(0, 1f)) //only use corn if it will heal. greater chance the lower the enemy health is
                {
                    float randomChanceToUseEachFrame = EatCornBaseChance * EnemyScalingFactor;
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
    [SerializeField] private float ShopChanceBase = 0.3f;
    [SerializeField] private int MaxPriceForNotShop = 100;
    [SerializeField] private float BaseCornIsShopChance = 0.4f;
    public bool ShouldItemDropAsShopItem(ItemData item)
    {
        if (item is Corn && BaseCornIsShopChance < Random.Range(0, 1f))
            return false;
        if (Random.Range(0, 1f) < ShopChanceBase)
            return true;
        if(item.Cost < Random.Range(0, MaxPriceForNotShop))
        {
            return false;
        }
        return true;
    }
    public bool DropItems(ref ItemData item)
    {
        if(item is not NoItem)
        {
            Vector2 random = new Vector2(1, 0).RotatedBy(Random.Range(0, Mathf.PI * 2));
            float dropChance = baseDropChance * Mathf.Sqrt(EnemyScalingFactor); //drop rates should get more common as you get into the game.
            if (item is Corn || dropChance > Random.Range(0, 1f)) //guaranteed to drop corn. Otherwise, drop rate is low
            {
                if(ShouldItemDropAsShopItem(item))
                {
                    Capsule.NewCapsule(item, transform.position);
                    return true;
                }
                else
                    ItemData.NewItem(item, transform.position, random);
            }
        }
        return false;
    }
    public override void OnDeath()
    {
        bool droppedShop = false;
        AudioManager.instance.Play("ZombieDeath");
        if (DropItems(ref LeftHeldItem))
            droppedShop = true;
        if (DropItems(ref RightHeldItem))
            droppedShop = true;
        if (!droppedShop)
        {
            GameObject coin = Instantiate(PrefabManager.GetPrefab("coin"), transform.position, new Quaternion());
            coin.GetComponent<Coin>().DespawnCounter = 0;
        }
    }
    public override void GenerateGore()
    {
        Gore.NewGore(CharacterAnimator.LeftArm);
        Gore.NewGore(CharacterAnimator.RightArm);
        Gore.NewGore(CharacterAnimator.BackLeg);
        Gore.NewGore(CharacterAnimator.FrontLeg);
        Gore.NewGore(CharacterAnimator.Head);
        Gore.NewGore(CharacterAnimator.Body);
    }
}
