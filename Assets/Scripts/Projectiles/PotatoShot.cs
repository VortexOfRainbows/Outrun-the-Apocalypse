using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoShot : ProjectileData
{
    public const int MaxLifetime = 50;
    public override string SpriteName => "Potato";
    public override void SetStats()
    {
        Size = new Vector2(20, 20); //Size of the hitbox in pixels
        Lifetime = MaxLifetime;
        Pierce = 1;
        Friendly = true;
    }
    public override void ModifyRenderer(ref SpriteRenderer Renderer)
    {
        Renderer.flipX = true;
    }
    public override void UpdateRenderer(ref SpriteRenderer Renderer)
    {
        Renderer.color = new Color(Renderer.color.r, Renderer.color.g, Renderer.color.b, Mathf.Sqrt(Lifetime / (float)MaxLifetime)); //so the projectile fades out slowly
    }
    public override void AfterSpawning(GameObject obj)
    {
        AudioManager.instance.Play("Shoot");
        obj.transform.localScale = new Vector3(1f, 0.90f, 1.0f);
    }
    private float BonusRotation = 0;
    public override void OnUpdate(ProjectileObject obj)
    {
        BonusRotation += obj.Velocity.magnitude * 0.05f;
        obj.transform.rotation = (obj.Velocity.ToRotation() + BonusRotation).ToQuaternion();
    }
}
