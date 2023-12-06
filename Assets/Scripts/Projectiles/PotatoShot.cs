using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoShot : ProjectileData
{
    public override string SpriteName => "Potato";
    public override void SetStats()
    {
        Size = new Vector2(20, 20); //Size of the hitbox in pixels
        Lifetime = 50;
        Pierce = 3;
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
