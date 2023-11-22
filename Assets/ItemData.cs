using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public abstract class ItemData
{
    public int UseCooldown;
    public int Damage;
    public float ShotVelocity;
    public ItemData()
    {
        Damage = -1;
        UseCooldown = 30;
        ShotVelocity = 10;
        SetStats();
    }
    public Vector2 LocalPosition;
    public void HoldingUpdate()
    {
        LocalPosition = HandOffset;
    }
    public void UseItem(Player player, HeldItem heldItem)
    {
        //Debug.Log(this + " " + "item used");
        if (CanUseItem())
        {
            Vector2 shootingPosition = (Vector2)heldItem.transform.position;
            Vector2 ToMouse = new Vector2(1, 0).RotatedBy(heldItem.transform.rotation[2]);
            Vector2 shootVelocity = ToMouse.normalized * ShotVelocity;
            int shootDamage = Damage;
            OnUseItem();
            bool UseDefaultShoot = Shoot(player, ref shootingPosition, ref shootVelocity, ref shootDamage);
            if(UseDefaultShoot)
            {
                FireProjectileTowardsCursor(player, shootingPosition, shootVelocity, shootDamage);
            }
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
    public static void FireProjectileTowardsCursor(Player player, Vector2 position, Vector2 velocity, int damage)
    {
        ProjectileData.NewProjectile(new BeastBlast(), position, velocity, damage);
    }
}