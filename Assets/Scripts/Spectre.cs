using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectre : ItemData
{
    public override void SetStats()
    {
        Damage = 1;
        ShotVelocity = 30.5f;
    }
    public override string SpriteName => "Spectre";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override Vector2 BarrelPosition => new Vector2(13.5f, 2.5f);
    public override bool ChangeHoldAnimation => true;
    public override float RotationOffset => -Mathf.PI / 2;
    public override bool HoldClick => true;
    public override ProjectileData ShootType => new SpectreShot();
    public override float UseCooldown => 9;
    public override bool Shoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        velocity = velocity.RotatedBy(Random.Range(-0.45f, 0.45f));
        position += velocity.normalized * 4f;
        return true;
    }
}
