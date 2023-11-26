using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ProjectileObject : MonoBehaviour
{
    public Vector2 Velocity;
    private Rigidbody2D RB;
    public SpriteRenderer Renderer;
    public ProjectileData Projectile;
    public void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
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
        Renderer.enabled = true;
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
    }
}
