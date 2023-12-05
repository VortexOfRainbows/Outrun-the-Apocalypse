using System;
using UnityEngine;

public abstract class ItemData
{
    public static void DropItem(ItemData itemData)
    {
        Vector2 toMouse = Utils.MouseWorld() - Player.MainPlayer.Position;
        NewItem(itemData, Player.MainPlayer.Position, toMouse.normalized * 8.5f);
    }
    public static GameObject NewItem(ItemData itemData, Vector2 position, Vector2 velocity)
    {
        GameObject itemObj = GameObject.Instantiate(PrefabManager.GetPrefab("item"));
        itemObj.transform.position = position;
        DroppedItem IOBJ = itemObj.GetComponent<DroppedItem>();
        IOBJ.Velocity = velocity;
        IOBJ.Item = itemData;
        return itemObj;
    }
    //This value is not serialized, because it is only used as a timer. It is not a value that needs to be changed
    private float CurrentCooldown; //Stores the current item cooldown time. Item can be not used when above 0
    ///
    /// The statistical values for this class are protected. This is so the child classes can modify them (but external classes cannot)
    /// 
    /// <summary>
    /// Whether the item is held out in the hand of the player
    /// </summary>
    public bool ChangeHoldAnimation { get; protected set; }
    /// <summary>
    /// If true, item can be used by holding click instead of repeatedly clicking
    /// </summary>
    public bool HoldClick { get; protected set; } 
    /// <summary>
    /// Rotation of the item when held by the player
    /// </summary>
    /// <summary>
    /// The amount of frame before this item is allowed to be used again after being used once
    /// </summary>
    protected float UseCooldown;
    public float RotationOffset { get; protected set; }
    public float Scale { get; protected set; }
    protected int Damage;
    protected float ShotVelocity;
    protected float Width; //These values determines the size of the item (hitbox)
    protected float Height; //Used when picking up an item from the floor
    /// <summary>
    /// The factor at which the item slows down when in world. Defaults to 0.94f
    /// </summary>
    protected float DeaccelerationRate;
    /// <summary>
    /// The projectile shot by the weapon on default. 
    /// This is perposefully a function rather than a field, as a new instance of the projectile type needs to be instantiated when a projectile is generated.
    /// </summary>
    protected virtual ProjectileData ShootType => new Bullet();
    public Vector2 Size
    {
        get
        {
            return new Vector2(Width, Height);
        }
        protected set
        {
            Width = value.x;
            Height = value.y;
        }
    }
    public ItemData()
    {
        DeaccelerationRate = 0.94f;
        UseCooldown = 20;
        ChangeHoldAnimation = false;
        HoldClick = true;
        RotationOffset = 0f;
        Scale = 0.75f;
        CurrentCooldown = 0;
        Damage = -1;
        ShotVelocity = 10;
        Size = new Vector2(32, 32);
        SetStats();
    }
    /// <summary>
    /// Whether or not the item should be consumed after being used. Defaults to False
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual bool ConsumeAfterUsing(Player player)
    {
        return false;
    }
    public bool CanUse()
    {
        return CanUseItem() && CurrentCooldown <= 0;
    }
    /// <summary>
    /// Called by the character animator when it is updating (whose update is called in the player)
    /// Performs basic Item update tasks
    /// </summary>
    public void HoldingUpdate()
    {
        if(CurrentCooldown > 0)
        {
            CurrentCooldown--;
        }
    }
    /// <summary>
    /// Performs all the item related use actions
    /// Returns true if the item should be consumed after being used
    /// </summary>
    /// <param name="item"></param>
    /// <param name="player"></param>
    /// <param name="heldItem"></param>
    /// <returns></returns>
    public bool UseItem(Player player, HeldItem heldItem)
    {
        //Debug.Log(this + " " + "item used");
        if (this.CanUse())
        {
            Vector2 shootingPosition = (Vector2)heldItem.transform.position;
            Vector2 ToMouse = new Vector2(1, 0).RotatedBy(heldItem.transform.eulerAngles.z * Mathf.Deg2Rad);

            float flippage = Mathf.Sign(heldItem.transform.parent.lossyScale.x);
            ToMouse *= flippage;

            Vector2 barrelOffset = this.BarrelPosition;
            barrelOffset.y *= flippage;
            shootingPosition += barrelOffset.RotatedBy(ToMouse.ToRotation());

            Vector2 shootVelocity = ToMouse.normalized * this.ShotVelocity;
            int shootDamage = this.Damage;
            this.OnUseItem();
            bool UseDefaultShoot = this.Shoot(player, ref shootingPosition, ref shootVelocity, ref shootDamage);
            if(UseDefaultShoot)
            {
                ProjectileData shootType = this.ShootType;
                FireProjectileTowardsCursor(shootType, shootingPosition, shootVelocity, shootDamage);
            }
            this.CurrentCooldown = this.UseCooldown;
            return this.ConsumeAfterUsing(player);
        }
        return false;
    }
    ///
    /// PLEASE READ THIS:
    /// WHY AM I USING LAMBDA FUNCTIONS?
    /// 
    /// Basically, these are functions instead of fields for future-proofing purposes.
    /// Some items may come with multiple sprites (but be within the same item)
    /// Having these work as functions allows for that functionality.
    /// An item could be programmed with an alternate state, and would simply change the sprite or SpriteName, then change the HandOffset/BarrelPosition according to the new sprite 
    /// 
    /// Here is an example of the utility of using these functions:
    /// 
    /// public override SpriteName => AlternateStateBool ? "AlternateSpriteName" : "DefaultSpriteName";
    /// 
    /// public override HandOffset => AlternateStateBool ? AlternateHandOffset : DefaultHandOffset;
    /// 
    /// public override BarrelPosition => AlternateStateBool ? AlternateBarrelVector: DefaultBarrelVector;
    /// 
    /// <summary>
    /// Fetches the sprite of the item. Only override this for specific purposes, such as when you want an item to be capable of having different sprites depending on the situation
    /// Defaults to:     SpriteLib.Library.GetSprite("Item", SpriteName)
    /// </summary>
    public virtual Sprite sprite => SpriteLib.Library.GetSprite("Item", SpriteName);
    public virtual string SpriteName { get; } //get functions work the same as lambda functions. And can be overriden in the same way
    /// <summary>
    /// Modifies the place where the gun sits in the players hand
    /// </summary>
    public virtual Vector2 HandOffset { get { return Vector2.zero; } }
    /// <summary>
    /// Modifies the place where projectile spawns when a gun fires
    /// </summary>
    public virtual Vector2 BarrelPosition { get { return Vector2.zero; } }
    /// <summary>
    /// This method is run when an item is spawned in
    /// Use this to set stats
    /// </summary>
    public virtual void SetStats()
    {

    }
    /// <summary>
    /// Return false to prevent an item from being useable
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseItem()
    {
        return true;
    }
    /// <summary>
    /// Allows you to make things happen when the item is used
    /// </summary>
    /// <returns></returns>
    public virtual void OnUseItem()
    {
        
    }
    /// <summary>
    /// Allows you to make an item launch a projectile when used
    /// Return true to make a projectile fire towards the cursor
    /// Stats changed here will have an effect on the normal projectile launch
    /// Return false to prevent a projectile from firing (instead, opting to make a custom pattern inside this method, for example)
    /// Returns false by default.
    /// </summary>
    /// <returns></returns>
    public virtual bool Shoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        return false;
    }
    public static void FireProjectileTowardsCursor(ProjectileData data, Vector2 position, Vector2 velocity, int damage)
    {
        ProjectileData.NewProjectile(data, position, velocity, damage);
    }
    /// <summary>
    /// Called once after initializing the item on the floor
    /// Use this for one-time modifications to the way the item is drawn on the floor
    /// </summary>
    /// <param name="Renderer"></param>
    public virtual void ModifyRenderer(ref SpriteRenderer Renderer)
    {

    }
    /// <summary>
    /// Called after initializing the item on the floor
    /// Use this for modifications to the way the item is drawn on the floor
    /// </summary>
    /// <param name="Renderer"></param>
    public virtual void UpdateRenderer(ref SpriteRenderer Renderer)
    {

    }
    public void Update(DroppedItem obj)
    {
        OnUpdate(obj);
        obj.Velocity *= DeaccelerationRate;
    }
    public virtual void OnUpdate(DroppedItem obj)
    {
        if(obj.Velocity != Vector2.zero)
            obj.transform.rotation = obj.Velocity.ToRotation().ToQuaternion();
    }
}