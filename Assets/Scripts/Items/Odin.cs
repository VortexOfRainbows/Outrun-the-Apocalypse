using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odin : ItemData
{
    public override void SetStats()
    {
        Size = new Vector2(51, 16);
        RotationOffset = -Mathf.PI / 2;
        ChangeHoldAnimation = true;
        HoldClick = true;
        UseCooldown = 6;
        Damage = 2;
        ShotVelocity = 40.5f;
    }
    public override string SpriteName => "Odin";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override Vector2 BarrelPosition => new Vector2(13.5f, 2.5f);
    protected override ProjectileData ShootType => new OdinShot();
    public override bool Shoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        position += velocity.normalized * 4f;
        velocity = velocity.RotatedBy(Random.Range(-0.3f, 0.3f));
        return true;
    }
}
