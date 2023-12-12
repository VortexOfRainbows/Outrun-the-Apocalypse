using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour
{
    public const float SecondsUntilEnemyStatsDouble = 600;
    public static float EnemyScalingFactor {
        get
        {
            float bonus = Timer.RawSeconds / SecondsUntilEnemyStatsDouble;
            if(bonus > 1)
            {
                bonus -= 1;
                bonus *= 0.5f; //any bonus to enemy difficulty after double becomes slower to acquire
                bonus += 1;
            }
            float num = 1 + bonus;
            return num;
        }
    }//Increases the difficulty of enemies as you gather more coins
    [SerializeField] protected GameObject HealthUI;
    [SerializeField] private Image HealthBar;
    //These field are public because they often need to be accessed by the character animator
    [SerializeField] public Rigidbody2D rb;
    public int Direction = 1;
    public int LastDirection = 1;
    public Vector2 PrevVelocity = Vector2.zero;
    public Vector2 Velocity = Vector2.zero;
    public virtual int LayerDefaultPosition => 0;
    public Vector2 LookTarget;

    public bool Friendly = true;
    public bool Hostile => !Friendly;

    [SerializeField] public float Life;
    [SerializeField] public float MaxLife;

    public const int TimeToDespawn = 600;
    protected int DespawnRange;
    private int Despawn;
    protected int DefaultImmunityOnHit;
    //Contact damage may be changed elsewhere, such as through environmental buffs, thus it is public. Immunity might be triggered by certain projectiles, thus it is also public
    public float ContactDamage;
    public int ImmunityFrames = 30;
    public bool JustSpawnedIn = true;
    public bool Immune => ImmunityFrames > 0 || JustSpawnedIn;
    private bool Dead;
    private void Start()
    {
        Dead = false;
        DespawnRange = 480; //480 is 16 pixels * 30 tiles away, some enemies may have differeing ranges
        Despawn = TimeToDespawn; //600 is the time to despawn
        ContactDamage = DefaultImmunityOnHit = ImmunityFrames = 0;
        Friendly = false;
        MaxLife = 10;
        ImmunityFrames = 30;
        SetStats();
        MaxLife *= EnemyScalingFactor;
        ContactDamage *= Mathf.Sqrt(EnemyScalingFactor);
        Life = MaxLife;
        JustSpawnedIn = false;
    }
    private void Update()
    {
        OnUpdate();
        if (HealthBar != null && HealthUI != null)
        {
            if (HealthBar.fillAmount != 1)
                HealthUI.SetActive(true);
            HealthBar.fillAmount = Life / MaxLife;
        }
    }
    private void FixedUpdate()
    {
        OnFixedUpdate();
        if (ImmunityFrames > 0)
        {
            ImmunityFrames--;
        }
        if (!(this is Player))
        {
            if ((Player.MainPlayer.Position - (Vector2)transform.position).magnitude > DespawnRange)
            {
                Despawn--;
            }
            else
                Despawn++;
            Despawn = Mathf.Clamp(Despawn, 0, TimeToDespawn);
            if(Despawn <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;
        if(collider.tag == "Projectile")
        {
            ProjectileObject projectileObject = collider.GetComponent<ProjectileObject>();
            if (projectileObject != null && !Immune && projectileObject.Projectile.Pierce > 0)
            {
                if ((Friendly && projectileObject.Projectile.Hostile) || (Hostile && projectileObject.Projectile.Friendly))
                {
                    int ImmunityFramesToTriggerOnDefault = DefaultImmunityOnHit;
                    projectileObject.Projectile.OnHit(projectileObject, this.gameObject, ref ImmunityFramesToTriggerOnDefault);
                    Hurt(projectileObject.Projectile.Damage, ImmunityFramesToTriggerOnDefault);
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;
        if (collider.tag == "Water")
        {
            rb.velocity *= 0.8f;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collider = collision.gameObject;
        if(collider.tag == "Player")
        {
            Entity entity = collider.GetComponent<Entity>();
            if (entity != null && ContactDamage > 0)
            {
                if(entity.Friendly && Hostile && !entity.Immune)
                {
                    entity.Hurt(ContactDamage, entity.DefaultImmunityOnHit);
                }
            }
        }
    }
    /// <summary>
    /// Call this to deal damage to an enemy/player
    /// </summary>
    /// <param name="damage"></param>
    public void Hurt(float damage, int setImmuneFrames) //This method is public as there may be some situations where damage should be done through other classes
    {
        DamageTextBehavior.SpawnDamageText(Mathf.CeilToInt(damage), transform.position + new Vector3(0, 12), Color.red); //Should spawn twelve pixels above center hit
        Life -= damage;
        if(Life <= 0)
        {
            Death();
        }
        if(this is Player)
        {
            AudioManager.instance.Play("Ow");
        }
        ImmunityFrames = setImmuneFrames;
    }
    public void Heal(int healAmount)
    {
        Life += healAmount;
        if (Life > MaxLife)
        {
            Life = MaxLife;
        }
        DamageTextBehavior.SpawnDamageText(Mathf.CeilToInt(healAmount), transform.position + new Vector3(0, 12), Color.green); //Should spawn twelve pixels above center hit
    }
    private void Death()
    {
        if(!Dead)
        {
            GenerateGore();
            OnDeath();
            if (this is Player)
            {
                UIManager.instance.GameOver();
            }
            else
                Destroy(this.gameObject);
            Dead = true;
        }
    }
    /// <summary>
    /// For entities, avoid running the normal update method as it might override entity functiosn
    /// use this method instead, as it runs after the normal entity methods
    /// </summary>
    public virtual void OnUpdate()
    {

    }
    /// <summary>
    /// For entities, avoid running the normal fixed update method as it might override entity functiosn
    /// use this method instead, as it runs after the normal entity methods
    /// </summary>
    public virtual void OnFixedUpdate()
    {

    }
    /// <summary>
    /// Called when an enemy or player dies. Put stuff here to make the enemy do special stuff upon death
    /// </summary>
    public virtual void OnDeath()
    {

    }
    /// <summary>
    /// Run once before any updates are called
    /// Use to set the basic stats of an enemy (life, maxlife, damage, etc)
    /// </summary>
    public virtual void SetStats()
    {

    }
    public virtual void GenerateGore()
    {

    }
}
public class EntityWithCharDrawing : Entity
{
    public ItemData LeftHeldItem;
    public ItemData RightHeldItem;
    public virtual float ArmDegreesOffset => 0f;
}