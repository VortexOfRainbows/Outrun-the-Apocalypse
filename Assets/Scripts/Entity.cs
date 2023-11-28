using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private GameObject HealthUI;
    [SerializeField] private Image HealthBar;
    //These field are public because they often need to be accessed by the character animator
    [SerializeField] public Rigidbody2D rb;
    public int Direction = 1;
    public int LastDirection = 1;
    public Vector2 PrevVelocity = Vector2.zero;
    public Vector2 Velocity = Vector2.zero;
    public virtual int LayerDefaultPosition => 0;
    public Vector2 LookTarget;

    public bool Friendly;
    public bool Hostile => !Friendly;
    public float Life { get; set; }
    public float MaxLife { get; set; } //The max life of an enemy/player will may need adjustment outside of spawning, particularly for player, thus public set
    private void Start()
    {
        Friendly = false;
        MaxLife = 10;
        Life = MaxLife;
        SetStats();
    }
    private void Update()
    {
        if(HealthBar != null && HealthUI != null)
        {
            if (HealthBar.fillAmount != 1)
                HealthUI.SetActive(true);
            HealthBar.fillAmount = Life / MaxLife;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;
        ProjectileObject projectileObject = collider.GetComponent<ProjectileObject>();
        Debug.Log(collision);
        if (projectileObject != null)
        {
            if((Friendly && projectileObject.Projectile.Hostile) || (Hostile && projectileObject.Projectile.Friendly))
            {
                Hurt(projectileObject.Projectile.Damage);
                projectileObject.Projectile.OnHit(projectileObject, this.gameObject);
            }
        }
    }
    /// <summary>
    /// Call this to deal damage to an enemy/player
    /// </summary>
    /// <param name="damage"></param>
    public void Hurt(int damage) //This method is public as there may be some situations where damage should be done through other classes
    {
        Life -= damage;
        if(Life <= 0)
        {
            Death();
        }
    }
    private void Death()
    {
        OnDeath();
        Destroy(this.gameObject);
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
}
public class EntityWithCharDrawing : Entity
{
    public ItemData LeftHeldItem;
    public ItemData RightHeldItem;
    public virtual float ArmDegreesOffset => 0f;
}