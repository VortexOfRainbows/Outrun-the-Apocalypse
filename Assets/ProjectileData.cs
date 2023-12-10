using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ProjectileData
{
    /// <summary>
    /// Instantiates a new projectile with the projectile data assigned. The projectile data is stored as a reference, so make sure to call *new ("projectiletype")*
    /// If you use the same reference for multiple instantiates, they will share the same projectile data (which I guess could be vaguely useful, but mostly should be avoided)
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="damage"></param>
    /// <returns></returns>
    public static GameObject NewProjectile(Entity owner, ProjectileData projectile, Vector2 position, Vector2 velocity, int damage, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0)
    {
        GameObject projectileObj = GameObject.Instantiate(PrefabManager.GetPrefab(0));
        projectileObj.transform.position = position;
        ProjectileObject POBJ = projectileObj.GetComponent<ProjectileObject>();
        POBJ.Velocity = velocity;
        POBJ.Projectile = projectile;
        projectile.Damage = damage;
        projectile.AI[0] = ai0;
        projectile.AI[1] = ai1;
        projectile.AI[2] = ai2;
        projectile.AI[3] = ai3;
        projectile.AfterSpawning(projectileObj);
        if(owner is Zombie z)
        {
            POBJ.Velocity *= z.projectileSpeedMultiplier;
            if(z.projectileSpeedMultiplier != 0)
            {
                projectile.Lifetime = (int)(projectile.Lifetime / z.projectileSpeedMultiplier);
            }
            if(projectile.Friendly)
            {
                projectile.Damage = (int)(projectile.Damage * Entity.EnemyScalingFactor); //damage should scale with the enemy scaling factor if it is a hostile projectile
                projectile.Friendly = false;
                projectile.Hostile = true;
                POBJ.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        return projectileObj;
    }
    /// <summary>
    /// the following fields are all public because they should be modifiable by other places in specific cases
    /// For example, certain enemies may have effects that lower the damage of bullets
    /// Some enemies may be able to multiple the size of other enemy bullets
    /// Etc.
    /// 
    /// Some enemies may decrease the lifetime of your own projectiles (decreasing their range), when they pierce (slimes, for example, might do that)
    /// </summary>
    public float Width; //These values determines the size of the projectile (hitbox)
    public float Height; //^^^^^
    public int Damage;
    public int Lifetime;
    public float[] AI; //These values can be used as general, all-purpose data storage for projectiles that desire unique update functionality. Though you can also designate your own variables too.
    public bool Hostile; //Whether or not this projectile should hurt friendly entities
    public bool Friendly; //Whether or not this projectile should hurt hostile entities
    public int Pierce; //can this projectile travel through enemies? How many time? Set to -1 in order to grant infinite pierce
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
    public ProjectileData()
    {
        Pierce = 1;
        Size = new Vector2(32, 32);
        Lifetime = 3600;
        Damage = 0;
        AI = new float[] { 0f, 0f, 0f, 0f };
        Friendly = Hostile = false;
        SetStats();
    }
    public void Update(ProjectileObject obj)
    {
        OnUpdate(obj);
        Lifetime--;
        if(Lifetime < 0)
        {
            Death(obj);
        }
    }
    public void OnHit(ProjectileObject obj, GameObject target, ref int ImmunityFrames)
    {
        Pierce--;
        OnHitTarget(obj, target);
        if(Pierce == 0)
        {
            Death(obj);
        }
    }
    private void Death(ProjectileObject obj)
    {
        OnDeath(obj);
        GameObject.Destroy(obj.gameObject);
    }
    public virtual Sprite sprite => SpriteLib.Library.GetSprite("Projectile", SpriteName);
    public virtual string SpriteName { get; }
    /// <summary>
    /// This method is run when a projectile is instantiated
    /// Use this to choose what stats the projectile should have.
    /// </summary>
    public virtual void SetStats()
    {

    }
    /// <summary>
    /// This method is run right after a projectile is spawned in
    /// Use this to choose what stats the projectile should have after being instantiated.
    /// In terms of run order, if damage is overriden here, then a projectile spawned with NewProjectile() will inherint the values in this method
    /// </summary>
    public virtual void AfterSpawning(GameObject obj)
    {
        
    }
    /// <summary>
    /// This method will run during the projectiles update cycle
    /// Velocity is part of ProjectileObject
    /// Position is part of the ProejctileObject
    /// </summary>
    public virtual void OnUpdate(ProjectileObject obj)
    {
        if (obj.Velocity != Vector2.zero)
            obj.transform.rotation = obj.Velocity.ToRotation().ToQuaternion();
    }
    /// <summary>
    /// This method will run right before the projectile is destroyed
    /// Velocity is part of ProjectileObject
    /// Position is part of the ProejctileObject
    /// </summary>
    public virtual void OnDeath(ProjectileObject obj)
    {

    }
    /// <summary>
    /// Called once after initializing the projectile
    /// Use this for one-time modifications to the way the projectile is drawn
    /// </summary>
    /// <param name="Renderer"></param>
    public virtual void ModifyRenderer(ref SpriteRenderer Renderer)
    {

    }
    /// <summary>
    /// Called continually after initializing the projectile
    /// Use this for modifications to the way the projectile is drawn
    /// </summary>
    /// <param name="Renderer"></param>
    public virtual void UpdateRenderer(ref SpriteRenderer Renderer)
    {

    }
    /// <summary>
    /// Called after hitting an enemy/player
    /// use this to give additional effects (besides damage) when a target is hit
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="target"></param>
    public virtual void OnHitTarget(ProjectileObject obj, GameObject target)
    {

    }
}
