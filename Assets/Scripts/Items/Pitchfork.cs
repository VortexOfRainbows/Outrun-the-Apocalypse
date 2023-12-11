using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitchfork : ItemData
{
    public override string StatObjName => "Pitchfork";
    public override string SpriteName => "Stab";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override Vector2 BarrelPosition => new Vector2(13.5f, 2.5f);
    protected override ProjectileData ShootType => new Stab();
    public override bool Shoot(Entity player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        position += velocity.normalized * 4f;
        return true;
    }
}
