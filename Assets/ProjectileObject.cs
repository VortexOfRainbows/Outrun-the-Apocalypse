using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    public Vector2 Velocity;
    public ProjectileData Projectile; //Public. So that ProjectileData can access this for its general purpose projectile-spawning method
    private Rigidbody2D RB;
    private SpriteRenderer Renderer;
    private BoxCollider2D Hitbox;
    public void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        Hitbox = GetComponent<BoxCollider2D>();
        Renderer.enabled = false;
    }
    public void Start()
    {
        if (Projectile == null)
        {
            Destroy(this.gameObject);
        }
        Renderer.sprite = Projectile.sprite;
        FixedUpdate();
        if(!Renderer.enabled)
        {
            Renderer.enabled = true;
            Projectile.ModifyRenderer(ref Renderer);
        }
    }
    private void FixedUpdate()
    {
        RB.velocity = Velocity;
        if(Projectile == null)
        {
            Destroy(gameObject);
            return;
        }
        Projectile.Update(this);
        Projectile.UpdateRenderer(ref Renderer);
        Hitbox.size = Projectile.Size;
    }
}
