using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class Key : ItemData
{
    public override string StatObjName => "Key";
    public override string SpriteName => "Key";
    public override Vector2 HandOffset => new Vector2(0f, -7.5f);
    protected override ProjectileData ShootType => new Stab();
    public float RandomSpread = 0.1f;
    public override bool Shoot(Entity player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        velocity = velocity.RotatedBy(Random.Range(-RandomSpread, RandomSpread));
        position += velocity.normalized * 2f;
        return true;
    }
    public override void OnHeldUpdate(Entity entity)
    {
        if(entity is Player player)
            player.HasKeyInHand = true;
    }
}
