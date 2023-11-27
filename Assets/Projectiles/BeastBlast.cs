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
    }
    public override void ModifyRenderer(ref SpriteRenderer Renderer)
    {
        Renderer.flipX = true;
    }
    public override void FinalSetStatsAfterSpawning(GameObject obj)
    {
        obj.transform.localScale = new Vector3(0.5f, 1.0f, 1.0f);
    }
}
public class BeastBlast : ProjectileData
{
    public override string SpriteName => "BeastShot";
    public override void SetStats()
    {
        Lifetime = 120;
    }
    public override void OnUpdate(ProjectileObject obj)
    {
        obj.transform.rotation = (obj.Velocity.ToRotation() - 90 * Mathf.Deg2Rad).ToQuaternion();
    }
}