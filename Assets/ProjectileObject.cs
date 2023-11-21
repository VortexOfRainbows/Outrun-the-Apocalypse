using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ProjectileObject : MonoBehaviour
{
    public Vector2 Velocity;
    public Rigidbody2D RB;
    public SpriteRenderer Renderer;
    public ProjectileData Projectile;
    public void Awake()
    {
        transform.localScale = Vector3.one;
        RB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        transform.localScale = Vector3.one;
        if (Projectile == null)
        {
            Destroy(this.gameObject);
        }
        Renderer.sprite = Projectile.sprite;
    }
    private void FixedUpdate()
    {
        Velocity = RB.velocity;
        Projectile.Update(this);
        RB.velocity = Velocity;
    }
}
