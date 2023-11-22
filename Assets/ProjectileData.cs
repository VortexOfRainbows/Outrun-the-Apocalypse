using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileData
{
    public int Damage;
    public int Lifetime;
    public float[] AI; //These values can be used as general, all-purpose data storage for projectiles that desire unique update functionality. Though you can also designate your own variables too.
    public ProjectileData()
    {
        Lifetime = 3600;
        Damage = 0;
        AI = new float[] { 0f, 0f, 0f, 0f };
        SetStats();
    }
    /// <summary>
    /// Instantiates a new projectile with the projectile data assigned. The projectile data is stored as a reference, so make sure to call *new ("projectiletype")*
    /// If you use the same reference for multiple instantiates, they will share the same projectile data (which I guess could be vaguely useful, but mostly should be avoided)
    /// </summary>
    /// <param name="projectileData"></param>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="damage"></param>
    /// <returns></returns>
    public static GameObject NewProjectile(ProjectileData projectileData, Vector2 position, Vector2 velocity, int damage, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0)
    {
        GameObject projectileObj = GameObject.Instantiate(PrefabManager.GetPrefab(0));
        projectileObj.transform.position = position;
        ProjectileObject POBJ = projectileObj.GetComponent<ProjectileObject>();
        POBJ.Velocity = velocity;
        POBJ.Projectile = projectileData;
        projectileData.Damage = damage;
        projectileData.AI[0] = ai0;
        projectileData.AI[1] = ai1;
        projectileData.AI[2] = ai2;
        projectileData.AI[3] = ai3;
        projectileData.FinalSetStatsAfterSpawning();
        return projectileObj;
    }
    public void Update(ProjectileObject obj)
    {
        OnUpdate(obj);
        Lifetime--;
        if(Lifetime < 0)
        {
            OnDeath(obj);
            GameObject.Destroy(obj.gameObject);
        }
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
    public virtual void FinalSetStatsAfterSpawning()
    {

    }
    /// <summary>
    /// This method will run during the projectiles update cycle
    /// Velocity is part of ProjectileObject
    /// Position is part of the ProejctileObject
    /// </summary>
    public virtual void OnUpdate(ProjectileObject obj)
    {

    }
    /// <summary>
    /// This method will run right before the projectile is destroyed
    /// Velocity is part of ProjectileObject
    /// Position is part of the ProejctileObject
    /// </summary>
    public virtual void OnDeath(ProjectileObject obj)
    {

    }
}
