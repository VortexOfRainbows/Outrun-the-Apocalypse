using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarShell : ProjectileData
{
    public override string SpriteName => "MortarShell";
    public override void SetStats()
    {
        Size = new Vector2(20, 20); //Size of the hitbox in pixels
        Lifetime = 90;
        Pierce = 2;
        Friendly = true;
        AudioManager.instance.Play("Shoot");
    }
    public override void ModifyRenderer(ref SpriteRenderer Renderer)
    {
        Renderer.flipX = true;
    }
    public override void AfterSpawning(GameObject obj)
    {
        obj.transform.localScale = new Vector3(1f, 0.90f, 1.0f);
    }
}