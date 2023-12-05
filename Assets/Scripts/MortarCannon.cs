using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarCannon : ItemData
{
    public override void SetStats()
    {
        Damage = 13;
        ShotVelocity = 10.5f;
    }
    public override string SpriteName => "MortarCannon";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override Vector2 BarrelPosition => new Vector2(13.5f, 2.5f);
    public override bool ChangeHoldAnimation => true;
    public override float RotationOffset => -Mathf.PI / 2;
    public override bool HoldClick => true;
    public override ProjectileData ShootType => new MortarShell();
    public override float UseCooldown => 60;
    public override bool Shoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        position += velocity.normalized * 4f;
        return true;
    }



}
