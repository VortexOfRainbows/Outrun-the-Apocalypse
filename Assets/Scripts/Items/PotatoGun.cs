using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoGun : ItemData
{
    public override void SetStats()
    {
        Damage = 100;
        ShotVelocity = 10.5f;
        RotationOffset = -Mathf.PI / 2;
        ChangeHoldAnimation = true;
        HoldClick = true; 
        UseCooldown = 350;
    }
    public override string SpriteName => "PotatoGun";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override Vector2 BarrelPosition => new Vector2(13.5f, 2.5f);
    protected override ProjectileData ShootType => new PotatoShot();
    public override bool Shoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        position += velocity.normalized * 4f;
        return true;
    }
}
