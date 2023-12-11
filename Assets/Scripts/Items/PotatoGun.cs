using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoGun : ItemData
{
    public override string StatObjName => "PotatoGun";
    public override string SpriteName => "PotatoGun";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override Vector2 BarrelPosition => new Vector2(19f, 4.5f);
    protected override ProjectileData ShootType => new PotatoShot();
    private const int PotatoTotal = 12;
    private const float DegreesSpread = 2.65f;
    private const float SpeedDeviation = 0.9f;
    public override bool Shoot(Entity player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        for (int i = -PotatoTotal; i <= PotatoTotal; i++)
        {
            float randomSpeedMult = Random.Range(1 - SpeedDeviation, 1 + SpeedDeviation);
            Vector2 perturbedSpeed = velocity.RotatedBy(Mathf.Deg2Rad * i * DegreesSpread * Random.Range(0, 1f)) * randomSpeedMult;
            ProjectileData.NewProjectile(player, ShootType, position + perturbedSpeed.normalized * 2, perturbedSpeed, damage);
        }
        return false;
    }
}
