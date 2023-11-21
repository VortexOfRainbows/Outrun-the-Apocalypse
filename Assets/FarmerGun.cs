using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class NoItem : ItemData
{
    public override string SpriteName => "None";
    public override Vector2 HandOffset => new Vector2(0, 0);
    public override bool ChangeHoldAnimation => false;
}
public class FarmerGun : ItemData
{
    public override void SetStats()
    {
        Damage = 5;
        ShotVelocity = 5.5f;
    }
    public override string SpriteName => "FarmerGun";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override bool ChangeHoldAnimation => true;
    public override float RotationOffset => -Mathf.PI / 2;
    public override bool HoldClick => false;
    public override bool Shoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        return true;
    }
}
