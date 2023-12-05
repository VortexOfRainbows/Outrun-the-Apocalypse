using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdinShot : ProjectileData
{
    public override string SpriteName => "Bullet";
    public override void SetStats()
    {
        Size = new Vector2(20, 20); //Size of the hitbox in pixels
        Lifetime = 90;
        Pierce = 1;
        Friendly = true;
        AudioManager.instance.Play("Shoot");
    }
    public override void ModifyRenderer(ref SpriteRenderer Renderer)
    {
        Renderer.flipX = true;
    }
    public override void FinalSetStatsAfterSpawning(GameObject obj)
    {
        obj.transform.localScale = new Vector3(1f, 0.90f, 1.0f);
    }
}
