using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SpectreShot : ProjectileData
{
    public override string SpriteName => "Bullet";
    public override void SetStats()
    {
        Size = new Vector2(20, 20); //Size of the hitbox in pixels
        Lifetime = 10;
        Pierce = 1;
        Friendly = true;
    }
    public override void ModifyRenderer(ref SpriteRenderer Renderer)
    {
        Renderer.flipX = true;
    }
    public override void AfterSpawning(GameObject obj)
    {
        AudioManager.instance.Play("Shoot");
        obj.transform.localScale = new Vector3(1f, 0.90f, 1.0f);
    }
}
