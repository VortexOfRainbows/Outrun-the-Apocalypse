using System;
using UnityEngine;

public abstract class ItemData
{
    public static GameObject NewItem(ItemData itemData, Vector2 position, Vector2 velocity)
    {
        GameObject itemObj = GameObject.Instantiate(PrefabManager.GetPrefab("item"));
        itemObj.transform.position = position;
        DroppedItem IOBJ = itemObj.GetComponent<DroppedItem>();
        IOBJ.Velocity = velocity;
        IOBJ.Item = itemData;
        return itemObj;
    }
    private float CurrentCooldown; //Stores the current item cooldown time. Item can be not used when above 0
    public int Damage;
    public float ShotVelocity;
    public float Width; //These values determines the size of the item (hitbox)
    public float Height; //Used when picking up an item from the floor
    public Vector2 Size
    {
        get
        {
            return new Vector2(Width, Height);
        }
        set
        {
            Width = value.x;
            Height = value.y;
        }
    }
    public ItemData()
    {
        CurrentCooldown = 0;
        Damage = -1;
        ShotVelocity = 10;
        Size = new Vector2(32, 32);
        SetStats();
    }
    public Vector2 LocalPosition;
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
        LocalPosition = HandOffset;
        if(CurrentCooldown > 0)
        {
            CurrentCooldown--;
        }
    }
    public void UseItem(Player player, HeldItem heldItem)
    {
        //Debug.Log(this + " " + "item used");
        if (CanUse())
        {
            Vector2 shootingPosition = (Vector2)heldItem.transform.position;
            Vector2 ToMouse = new Vector2(1, 0).RotatedBy(heldItem.transform.eulerAngles.z * Mathf.Deg2Rad);

            float flippage = Mathf.Sign(heldItem.transform.parent.lossyScale.x);
            ToMouse *= flippage;

            Vector2 barrelOffset = this.BarrelPosition;
            barrelOffset.y *= flippage;
            shootingPosition += barrelOffset.RotatedBy(ToMouse.ToRotation());

            Vector2 shootVelocity = ToMouse.normalized * ShotVelocity;
            int shootDamage = Damage;
            OnUseItem();
            bool UseDefaultShoot = Shoot(player, ref shootingPosition, ref shootVelocity, ref shootDamage);
            if(UseDefaultShoot)
            {
                FireProjectileTowardsCursor(ShootType, shootingPosition, shootVelocity, shootDamage);
            }
            CurrentCooldown = UseCooldown;
        }
    }
    /// <summary>
    /// Fetches the sprite of the item. Only override this for specific purposes, such as when you want an item to be capable of having different sprites depending on the situation
    /// Defaults to:     SpriteLib.Library.GetSprite("Item", SpriteName)
    /// </summary>
    public virtual Sprite sprite => SpriteLib.Library.GetSprite("Item", SpriteName);

    public virtual string SpriteName { get; }
    /// <summary>
    /// Modifies the place where the gun sits in the players hand
    /// </summary>
    public virtual Vector2 HandOffset { get; }
    /// <summary>
    /// Modifies the place where projectile spawns when a gun fires
    /// </summary>
    public virtual Vector2 BarrelPosition { get; }
    public virtual bool ChangeHoldAnimation => false;
    /// <summary>
    /// If true, item can be used by holding click instead of repeatedly clicking
    /// </summary>
    public virtual bool HoldClick => false;
    public virtual float RotationOffset => 0f;
    public virtual float Scale => 0.75f;
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
    public virtual ProjectileData ShootType => new Bullet();
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
    /// The amount of frame before this item is allowed to be used again after being used once
    /// </summary>
    public virtual float UseCooldown => 20f;
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
    /// <summary>
    /// The factor at which the item slows down when in world
    /// </summary>
    public virtual float DeaccelerationRate => 0.94f;
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