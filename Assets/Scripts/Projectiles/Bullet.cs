using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ProjectileData
{
    public override string SpriteName => "Bullet";
    public override void SetStats()
    {
        Size = new Vector2(40, 1); //Size of the hitbox in pixels
        Lifetime = 60;
        Friendly = true;
    }
    public override void ModifyRenderer(ref SpriteRenderer Renderer)
    {
        Renderer.flipX = true;
    }
    public override void AfterSpawning(GameObject obj)
    {
        AudioManager.instance.Play("Shoot");
        obj.transform.localScale = new Vector3(0.5f, 1.0f, 1.0f);
    }
}