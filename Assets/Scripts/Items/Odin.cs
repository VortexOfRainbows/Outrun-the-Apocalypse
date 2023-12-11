using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odin : ItemData
{
    public override string StatObjName => "Odin";
    public override string SpriteName => "Odin";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override Vector2 BarrelPosition => new Vector2(25f, 2.5f);
    protected override ProjectileData ShootType => new OdinShot();
    private const float Spread = 0.2f;
    public override bool Shoot(Entity player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        position += velocity.normalized * 2f;
        velocity = velocity.RotatedBy(Random.Range(-Spread, Spread));
        return true;
    }
}
