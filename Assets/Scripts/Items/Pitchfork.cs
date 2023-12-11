using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Pitchfork : ItemData
{
    public override string StatObjName => "Pitchfork";
    public override string SpriteName => "Pitchfork";
    public override Vector2 HandOffset => new Vector2(0f, 0f);
    public override Vector2 BarrelPosition => new Vector2(30, 0);
    protected override ProjectileData ShootType => new Stab();
    private const float DegreesSpread = 25;
    public override bool Shoot(Entity player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        for(int i = -1; i <= 1; i++)
        {
            Vector2 perturbedSpeed = velocity.RotatedBy(Mathf.Deg2Rad * i * DegreesSpread);
            ProjectileData.NewProjectile(player, ShootType, position + perturbedSpeed.normalized * 6, perturbedSpeed, damage);
        }
        return false;
    }
}
