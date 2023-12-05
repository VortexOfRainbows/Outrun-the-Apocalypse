using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectre : ItemData
{
    public override void SetStats()
    {
        Size = new Vector2(34, 12);
        Damage = 1;
        ShotVelocity = 30.5f;
        RotationOffset = -Mathf.PI / 2;
        ChangeHoldAnimation = true;
        HoldClick = true;
        UseCooldown = 9;
    }
    public override string SpriteName => "Spectre";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override Vector2 BarrelPosition => new Vector2(13.5f, 2.5f);
    protected override ProjectileData ShootType => new SpectreShot();
    public override bool Shoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        velocity = velocity.RotatedBy(Random.Range(-0.45f, 0.45f));
        position += velocity.normalized * 4f;
        return true;
    }
}
