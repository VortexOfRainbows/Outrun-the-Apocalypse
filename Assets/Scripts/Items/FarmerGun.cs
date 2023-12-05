using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class FarmerGun : ItemData
{
    public override void SetStats()
    {
        Damage = 5;
        ShotVelocity = 12.5f;
        Size = new Vector2(17, 8);
        RotationOffset = -Mathf.PI / 2;
        ChangeHoldAnimation = true;
        HoldClick = true;
    }
    public override string SpriteName => "FarmerGun";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override Vector2 BarrelPosition => new Vector2(13.5f, 2.5f);
    protected override ProjectileData ShootType => base.ShootType;
    public override bool Shoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        position += velocity.normalized * 4f;
        return true;
    }
}
